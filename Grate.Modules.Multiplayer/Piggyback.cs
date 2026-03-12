using System;
using GorillaLocomotion;
using Grate.Extensions;
using Grate.GUI;
using Grate.Gestures;
using Grate.Modules.Physics;
using Grate.Networking;
using Grate.Tools;
using Photon.Pun;
using UnityEngine;

namespace Grate.Modules.Multiplayer;

public class Piggyback : GrateModule
{
	public struct RigScanResult
	{
		public Transform transform;

		public VRRig rig;

		public float distance;
	}

	private const float mountDistance = 1.5f;

	public static readonly string DisplayName = "Piggyback";

	public static bool mounted;

	public static Piggyback Instance;

	private readonly Vector3 mountOffset = new Vector3(0f, 1f, -1f);

	private bool latchedWithLeft;

	private Transform mount;

	private VRRig mountedRig;

	private Vector3 mountPosition;

	private void Awake()
	{
		Instance = this;
	}

	protected override void Start()
	{
		base.Start();
	}

	private void FixedUpdate()
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		if (mounted)
		{
			if (RevokingConsent(mountedRig))
			{
				Unmount();
				GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(98, false, 1f);
			}
			else
			{
				mount.TransformPoint(mountOffset);
				GTPlayer.Instance.TeleportTo(mount, true, true);
			}
		}
	}

	protected override void OnEnable()
	{
		if (MenuController.Instance.Built)
		{
			base.OnEnable();
			InputTracker<float>? leftGrip = GestureTracker.Instance.leftGrip;
			leftGrip.OnPressed = (Action<InputTracker>)Delegate.Combine(leftGrip.OnPressed, new Action<InputTracker>(Latch));
			InputTracker<float>? leftGrip2 = GestureTracker.Instance.leftGrip;
			leftGrip2.OnReleased = (Action<InputTracker>)Delegate.Combine(leftGrip2.OnReleased, new Action<InputTracker>(Unlatch));
			InputTracker<float>? rightGrip = GestureTracker.Instance.rightGrip;
			rightGrip.OnPressed = (Action<InputTracker>)Delegate.Combine(rightGrip.OnPressed, new Action<InputTracker>(Latch));
			InputTracker<float>? rightGrip2 = GestureTracker.Instance.rightGrip;
			rightGrip2.OnReleased = (Action<InputTracker>)Delegate.Combine(rightGrip2.OnReleased, new Action<InputTracker>(Unlatch));
		}
	}

	private void Mount(Transform t, VRRig rig)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		mountPosition = ((Component)GTPlayer.Instance.bodyCollider).transform.position;
		mountedRig = rig;
		mounted = true;
		mount = t;
		EnableNoClip();
	}

	private void Unmount()
	{
		mount = null;
		mounted = false;
		mountedRig = null;
		mount = null;
		DisableNoClip();
		((MonoBehaviour)this).Invoke("WarpBack", 0.05f);
	}

	private void WarpBack()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		GTPlayer.Instance.TeleportTo(mountPosition, GTPlayer.Instance.turnParent.transform.rotation, false, false);
	}

	public RigScanResult ClosestRig(Transform hand)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		VRRig rig = null;
		Transform transform = null;
		float num = float.PositiveInfinity;
		foreach (VRRig vrrig in ((GorillaParent)GorillaParent.instance).vrrigs)
		{
			try
			{
				if (!vrrig.OwningNetPlayer.IsLocal)
				{
					Transform val = OVRExtensions.FindChildRecursive(((Component)vrrig).transform, "head");
					float num2 = Vector3.Distance(hand.position, val.position);
					if (num2 < num)
					{
						num = num2;
						transform = val;
						rig = vrrig;
					}
				}
			}
			catch (Exception e)
			{
				Logging.Exception(e);
			}
		}
		RigScanResult result = default(RigScanResult);
		result.transform = transform;
		result.distance = num;
		result.rig = rig;
		return result;
	}

	private bool GivingConsent(VRRig rig)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		NetworkedPlayer component = ((Component)rig).GetComponent<NetworkedPlayer>();
		if (NetPlayer.op_Implicit(PhotonNetwork.LocalPlayer).IsAdmin())
		{
			return true;
		}
		if (!component.RightTriggerPressed || !component.RightGripPressed || !(component.RightThumbAmount < 0.25f) || !(Vector3.Dot(Vector3.up, rig.rightHandTransform.forward) > 0f))
		{
			if (component.LeftTriggerPressed && component.LeftGripPressed && component.LeftThumbAmount < 0.25f)
			{
				return Vector3.Dot(Vector3.up, rig.leftHandTransform.forward) > 0f;
			}
			return false;
		}
		return true;
	}

	private bool RevokingConsent(VRRig rig)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		NetworkedPlayer component = ((Component)rig).GetComponent<NetworkedPlayer>();
		if (NetPlayer.op_Implicit(PhotonNetwork.LocalPlayer).IsAdmin())
		{
			return false;
		}
		if (!component.RightTriggerPressed || !component.RightGripPressed || !(component.RightThumbAmount < 0.25f) || !(Vector3.Dot(Vector3.down, rig.rightHandTransform.forward) > 0f))
		{
			if (component.LeftTriggerPressed && component.LeftGripPressed && component.LeftThumbAmount < 0.25f)
			{
				return Vector3.Dot(Vector3.down, rig.leftHandTransform.forward) > 0f;
			}
			return false;
		}
		return true;
	}

	private void EnableNoClip()
	{
		NoClip component = ((Component)Plugin.MenuController).GetComponent<NoClip>();
		component.button.AddBlocker(ButtonController.Blocker.PIGGYBACKING);
		((Behaviour)component).enabled = true;
	}

	private void DisableNoClip()
	{
		NoClip component = ((Component)Plugin.MenuController).GetComponent<NoClip>();
		component.button.RemoveBlocker(ButtonController.Blocker.PIGGYBACKING);
		((Behaviour)component).enabled = false;
	}

	private bool TryMount(bool isLeft)
	{
		GameObject val = (isLeft ? GestureTracker.Instance.leftHand : GestureTracker.Instance.rightHand);
		RigScanResult rigScanResult = ClosestRig(val.transform);
		if (rigScanResult.distance < 1.5f && ((Behaviour)this).enabled && !mounted && GivingConsent(rigScanResult.rig))
		{
			if (!PositionValidator.Instance.isValidAndStable)
			{
				GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(68, false, 1f);
				return false;
			}
			Mount(rigScanResult.rig.headConstraint, rigScanResult.rig);
			return true;
		}
		return false;
	}

	private void Latch(InputTracker input)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		if ((int)input.node == 4)
		{
			latchedWithLeft = TryMount(isLeft: true);
		}
		else
		{
			latchedWithLeft = !TryMount(isLeft: false);
		}
	}

	private void Unlatch(InputTracker input)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Invalid comparison between Unknown and I4
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		if (((Behaviour)this).enabled && mounted && (((int)input.node == 4 && latchedWithLeft) || ((int)input.node == 5 && !latchedWithLeft)))
		{
			Unmount();
		}
	}

	protected override void Cleanup()
	{
		if (MenuController.Instance.Built)
		{
			if (mounted)
			{
				Unmount();
			}
			if (GestureTracker.Instance != null)
			{
				InputTracker<float>? leftGrip = GestureTracker.Instance.leftGrip;
				leftGrip.OnPressed = (Action<InputTracker>)Delegate.Remove(leftGrip.OnPressed, new Action<InputTracker>(Latch));
				InputTracker<float>? leftGrip2 = GestureTracker.Instance.leftGrip;
				leftGrip2.OnReleased = (Action<InputTracker>)Delegate.Remove(leftGrip2.OnReleased, new Action<InputTracker>(Unlatch));
				InputTracker<float>? rightGrip = GestureTracker.Instance.rightGrip;
				rightGrip.OnPressed = (Action<InputTracker>)Delegate.Remove(rightGrip.OnPressed, new Action<InputTracker>(Latch));
				InputTracker<float>? rightGrip2 = GestureTracker.Instance.rightGrip;
				rightGrip2.OnReleased = (Action<InputTracker>)Delegate.Remove(rightGrip2.OnReleased, new Action<InputTracker>(Unlatch));
			}
		}
	}

	public override string GetDisplayName()
	{
		return DisplayName;
	}

	public override string Tutorial()
	{
		return "- To mount a player, ask them to give you a thumbs up.\n- Hold down [Grip] near their head to hop on.\n- If the player gives you a thumbs down you will be dismounted.";
	}
}
