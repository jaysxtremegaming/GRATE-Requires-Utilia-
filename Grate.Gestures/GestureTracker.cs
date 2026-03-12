using System;
using System.Collections.Generic;
using System.Linq;
using GorillaLocomotion;
using Grate.Extensions;
using Grate.Tools;
using HarmonyLib;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace Grate.Gestures;

public class GestureTracker : MonoBehaviour
{
	public struct BodyVectors
	{
		public Vector3 pointerDirection;

		public Vector3 palmNormal;

		public Vector3 thumbDirection;
	}

	public const string localRigPath = "Player Objects/Local VRRig/Local Gorilla Player";

	public const string palmPath = "/rig/hand.{0}/palm.01.{0}";

	public const string pointerFingerPath = "/rig/hand.{0}/palm.01.{0}/f_index.01.{0}/f_index.02.{0}/f_index.03.{0}";

	public const string thumbPath = "/rig/hand.{0}/palm.01.{0}/thumb.01.{0}/thumb.02.{0}/thumb.03.{0}";

	public static GestureTracker Instance;

	private readonly float illProximityThreshold = 0.1f;

	private readonly Queue<int> meatBeatCollisions = new Queue<int>();

	public float camOffset = -45f;

	public GameObject chest;

	public GameObject leftPointerObj;

	public GameObject rightPointerObj;

	public GameObject leftHand;

	public GameObject rightHand;

	public List<InputTracker?> inputs;

	public bool isIlluminatiing;

	public bool isChargingKamehameha;

	private float lastBeat;

	public InputDevice leftController;

	public InputDevice rightController;

	public InputTracker<float>? leftGrip;

	public BodyVectors leftHandVectors;

	public BodyVectors rightHandVectors;

	public BodyVectors headVectors;

	public GrateInteractor leftPalmInteractor;

	public GrateInteractor rightPalmInteractor;

	public GrateInteractor leftPointerInteractor;

	public GrateInteractor rightPointerInteractor;

	public Transform leftPointerTransform;

	public Transform rightPointerTransform;

	public Transform leftThumbTransform;

	public Transform rightThumbTransform;

	public InputTracker<bool>? leftPrimary;

	public InputTracker<bool>? leftSecondary;

	public InputTracker<bool>? leftStick;

	public InputTracker<Vector2>? leftStickAxis;

	public InputTracker<float>? leftTrigger;

	public Action<Vector3> OnGlide;

	public Action OnIlluminati;

	public Action OnKamehameha;

	public Action OnMeatBeat;

	public InputTracker<float>? rightGrip;

	public InputTracker<bool>? rightPrimary;

	public InputTracker<bool>? rightSecondary;

	public InputTracker<bool>? rightStick;

	public InputTracker<Vector2>? rightStickAxis;

	public InputTracker<float>? rightTrigger;

	private void Awake()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		Logging.Debug("Awake");
		Instance = this;
		leftController = InputDevices.GetDeviceAtXRNode((XRNode)4);
		rightController = InputDevices.GetDeviceAtXRNode((XRNode)5);
		Traverse val = Traverse.Create((object)ControllerInputPoller.instance);
		Traverse val2 = Traverse.Create((object)new ControllerInputPollerExt());
		leftGrip = new InputTracker<float>(val.Field("leftControllerGripFloat"), (XRNode)4);
		rightGrip = new InputTracker<float>(val.Field("rightControllerGripFloat"), (XRNode)5);
		leftTrigger = new InputTracker<float>(val.Field("leftControllerIndexFloat"), (XRNode)4);
		rightTrigger = new InputTracker<float>(val.Field("rightControllerIndexFloat"), (XRNode)5);
		leftPrimary = new InputTracker<bool>(val.Field("leftControllerPrimaryButton"), (XRNode)4);
		rightPrimary = new InputTracker<bool>(val.Field("rightControllerPrimaryButton"), (XRNode)5);
		leftSecondary = new InputTracker<bool>(val.Field("leftControllerSecondaryButton"), (XRNode)4);
		rightSecondary = new InputTracker<bool>(val.Field("rightControllerSecondaryButton"), (XRNode)5);
		leftStick = new InputTracker<bool>(val2.Field("leftControllerStickButton"), (XRNode)4);
		rightStick = new InputTracker<bool>(val2.Field("rightControllerStickButton"), (XRNode)5);
		leftStickAxis = new InputTracker<Vector2>(val2.Field("leftControllerStickAxis"), (XRNode)4);
		rightStickAxis = new InputTracker<Vector2>(val2.Field("rightControllerStickAxis"), (XRNode)5);
		inputs = new List<InputTracker>
		{
			leftGrip, rightGrip, leftTrigger, rightTrigger, leftPrimary, rightPrimary, leftSecondary, rightSecondary, leftStick, rightStick,
			leftStickAxis, rightStickAxis
		};
		BuildColliders();
		CollisionObserver collisionObserver = chest.AddComponent<CollisionObserver>();
		collisionObserver.OnTriggerEntered = (Action<GameObject, Collider>)Delegate.Combine(collisionObserver.OnTriggerEntered, new Action<GameObject, Collider>(OnChestBeat));
	}

	private void Update()
	{
		ControllerInputPollerExt.Instance.Update();
		UpdateValues();
		TrackBodyVectors();
		TrackGlideGesture();
		isIlluminatiing = TrackIlluminatiGesture();
		isChargingKamehameha = TrackKamehamehaGesture();
	}

	private void FixedUpdate()
	{
		if (Time.time - lastBeat > 1f)
		{
			meatBeatCollisions.Clear();
		}
	}

	public void OnDestroy()
	{
		Logging.Debug("Gesture Tracker Destroy");
		leftHand?.Obliterate();
		rightHand?.Obliterate();
		leftPointerObj?.Obliterate();
		rightPointerObj?.Obliterate();
		Instance = null;
		OnMeatBeat = null;
	}

	public void UpdateValues()
	{
		if (!NetworkSystem.Instance.InRoom)
		{
			return;
		}
		if (NetworkSystem.Instance.GameModeString.Contains("MODDED_"))
		{
			foreach (InputTracker input in inputs)
			{
				input.UpdateValues();
			}
			return;
		}
		NetworkSystem.Instance.ReturnToSinglePlayer();
	}

	private void TrackBodyVectors()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		Transform transform = leftHand.transform;
		leftHandVectors = new BodyVectors
		{
			pointerDirection = transform.forward,
			palmNormal = transform.right,
			thumbDirection = transform.up
		};
		Transform transform2 = rightHand.transform;
		rightHandVectors = new BodyVectors
		{
			pointerDirection = transform2.forward,
			palmNormal = transform2.right * -1f,
			thumbDirection = transform2.up
		};
		Transform transform3 = ((Component)GTPlayer.Instance.headCollider).transform;
		headVectors = new BodyVectors
		{
			pointerDirection = transform3.forward,
			palmNormal = transform3.right,
			thumbDirection = transform3.up
		};
	}

	private bool TrackIlluminatiGesture()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		float scale = GTPlayer.Instance.scale;
		if (Vector3.Distance(leftPointerTransform.position, rightPointerTransform.position) > illProximityThreshold * scale)
		{
			return false;
		}
		if (Vector3.Distance(leftThumbTransform.position, rightThumbTransform.position) > illProximityThreshold * scale)
		{
			return false;
		}
		if (PalmsFacingSameWay())
		{
			OnIlluminati?.Invoke();
			return true;
		}
		return false;
	}

	private bool TrackKamehamehaGesture()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		float scale = GTPlayer.Instance.scale;
		if (Vector3.Distance(((Component)leftPalmInteractor).transform.position, ((Component)rightPalmInteractor).transform.position) > 0.25f * scale)
		{
			return false;
		}
		if (PalmsFacingEachOther() && FingersFacingAway())
		{
			OnKamehameha?.Invoke();
			return true;
		}
		return false;
	}

	private void TrackGlideGesture()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		if (FingersFacingAway() && PalmsFacingSameWay())
		{
			Vector3 val = (leftHandVectors.thumbDirection + rightHandVectors.thumbDirection) / 2f;
			if (Vector3.Dot(val, headVectors.pointerDirection) > 0f)
			{
				OnGlide?.Invoke(val);
			}
		}
	}

	public bool PalmsFacingEachOther()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		if (leftHand.transform.InverseTransformPoint(rightHand.transform.position).x < 0f)
		{
			return false;
		}
		return Vector3.Dot(leftHandVectors.palmNormal, rightHandVectors.palmNormal) < -0.5f;
	}

	public bool PalmsFacingSameWay()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		return Vector3.Dot(leftHandVectors.palmNormal, rightHandVectors.palmNormal) > 0.5f;
	}

	public bool FingersFacingAway()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		if (leftHand.transform.InverseTransformPoint(rightHand.transform.position).z > 0f)
		{
			return false;
		}
		return Vector3.Dot(leftHandVectors.pointerDirection, rightHandVectors.pointerDirection) < -0.5f;
	}

	private void OnChestBeat(GameObject obj, Collider collider)
	{
		if (!XRSettings.isDeviceActive || ((Object)(object)((Component)collider).gameObject != (Object)(object)leftHand && (Object)(object)((Component)collider).gameObject != (Object)(object)rightHand))
		{
			return;
		}
		lastBeat = Time.time;
		if (meatBeatCollisions.Count > 3)
		{
			meatBeatCollisions.Dequeue();
		}
		if ((Object)(object)((Component)collider).gameObject == (Object)(object)leftHand)
		{
			meatBeatCollisions.Enqueue(0);
		}
		else if ((Object)(object)((Component)collider).gameObject == (Object)(object)rightHand)
		{
			meatBeatCollisions.Enqueue(1);
		}
		if (meatBeatCollisions.Count < 4)
		{
			return;
		}
		int num = -1;
		for (int i = 0; i < meatBeatCollisions.Count; i++)
		{
			int num2 = meatBeatCollisions.ElementAt(i);
			if (num == num2)
			{
				return;
			}
			num = num2;
		}
		meatBeatCollisions.Clear();
		OnMeatBeat?.Invoke();
	}

	private void BuildColliders()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		Logging.Debug("BuildColliders");
		GTPlayer instance = GTPlayer.Instance;
		chest = new GameObject("Body Gesture Collider");
		((Collider)chest.AddComponent<CapsuleCollider>()).isTrigger = true;
		chest.AddComponent<Rigidbody>().isKinematic = true;
		chest.transform.SetParent(OVRExtensions.FindChildRecursive(((Component)instance).transform, "Body Collider"), false);
		chest.layer = LayerMask.NameToLayer("Water");
		float num = 0.125f;
		float num2 = 0.25f;
		chest.transform.localScale = new Vector3(num2, num, num2);
		Transform transform = GameObject.Find(string.Format("Player Objects/Local VRRig/Local Gorilla Player/rig/hand.{0}/palm.01.{0}", "L")).transform;
		leftPalmInteractor = CreateInteractor("Left Palm Interactor", transform, 0.0625f);
		leftHand = ((Component)leftPalmInteractor).gameObject;
		leftHand.transform.localRotation = Quaternion.Euler(-90f, -90f, 0f);
		Transform transform2 = GameObject.Find(string.Format("Player Objects/Local VRRig/Local Gorilla Player/rig/hand.{0}/palm.01.{0}", "R")).transform;
		rightPalmInteractor = CreateInteractor("Right Palm Interactor", transform2, 0.0625f);
		rightHand = ((Component)rightPalmInteractor).gameObject;
		rightHand.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
		leftPointerTransform = GameObject.Find(string.Format("Player Objects/Local VRRig/Local Gorilla Player/rig/hand.{0}/palm.01.{0}/f_index.01.{0}/f_index.02.{0}/f_index.03.{0}", "L")).transform;
		leftPointerInteractor = CreateInteractor("Left Pointer Interactor", leftPointerTransform, 1f / 32f);
		leftPointerObj = ((Component)leftPointerInteractor).gameObject;
		rightPointerTransform = GameObject.Find(string.Format("Player Objects/Local VRRig/Local Gorilla Player/rig/hand.{0}/palm.01.{0}/f_index.01.{0}/f_index.02.{0}/f_index.03.{0}", "R")).transform;
		rightPointerInteractor = CreateInteractor("Right Pointer Interactor", rightPointerTransform, 1f / 32f);
		rightPointerObj = ((Component)rightPointerInteractor).gameObject;
		leftThumbTransform = GameObject.Find(string.Format("Player Objects/Local VRRig/Local Gorilla Player/rig/hand.{0}/palm.01.{0}/thumb.01.{0}/thumb.02.{0}/thumb.03.{0}", "L")).transform;
		rightThumbTransform = GameObject.Find(string.Format("Player Objects/Local VRRig/Local Gorilla Player/rig/hand.{0}/palm.01.{0}/thumb.01.{0}/thumb.02.{0}/thumb.03.{0}", "R")).transform;
	}

	public GrateInteractor CreateInteractor(string name, Transform parent, float scale)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		GameObject val = new GameObject(name);
		GrateInteractor result = val.AddComponent<GrateInteractor>();
		val.transform.SetParent(parent, false);
		val.transform.localScale = Vector3.one * scale;
		return result;
	}

	public XRController GetController(bool isLeft)
	{
		XRController[] array = Object.FindObjectsOfType<XRController>();
		foreach (XRController val in array)
		{
			if (isLeft && ((Object)val).name.ToLowerInvariant().Contains("left"))
			{
				return val;
			}
			if (!isLeft && ((Object)val).name.ToLowerInvariant().Contains("right"))
			{
				return val;
			}
		}
		return null;
	}

	public void HapticPulse(bool isLeft, float strength = 0.5f, float duration = 0.05f)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		InputDevice val = (isLeft ? leftController : rightController);
		((InputDevice)(ref val)).SendHapticImpulse(0u, strength, duration);
	}

	public InputTracker? GetInputTracker(string name, XRNode node)
	{
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Invalid comparison between Unknown and I4
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Invalid comparison between Unknown and I4
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Invalid comparison between Unknown and I4
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Invalid comparison between Unknown and I4
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Invalid comparison between Unknown and I4
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Invalid comparison between Unknown and I4
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Invalid comparison between Unknown and I4
		switch (name)
		{
		case "grip":
			if ((int)node != 4)
			{
				return rightGrip;
			}
			return leftGrip;
		case "trigger":
			if ((int)node != 4)
			{
				return rightTrigger;
			}
			return leftTrigger;
		case "stick":
			if ((int)node != 4)
			{
				return rightStick;
			}
			return leftStick;
		case "primary":
			if ((int)node != 4)
			{
				return rightPrimary;
			}
			return leftPrimary;
		case "secondary":
			if ((int)node != 4)
			{
				return rightSecondary;
			}
			return leftSecondary;
		case "a/x":
			if ((int)node != 4)
			{
				return rightPrimary;
			}
			return leftPrimary;
		case "b/y":
			if ((int)node != 4)
			{
				return rightSecondary;
			}
			return leftSecondary;
		case "a":
			return rightPrimary;
		case "b":
			return rightSecondary;
		case "x":
			return leftPrimary;
		case "y":
			return leftSecondary;
		default:
			return null;
		}
	}
}
