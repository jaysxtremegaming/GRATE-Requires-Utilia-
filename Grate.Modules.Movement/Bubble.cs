using System;
using BepInEx.Configuration;
using GorillaLocomotion;
using Grate.Extensions;
using Grate.GUI;
using Grate.Gestures;
using Grate.Networking;
using Grate.Patches;
using Grate.Tools;
using UnityEngine;

namespace Grate.Modules.Movement;

public class Bubble : GrateModule
{
	public static readonly string DisplayName = "Bubble";

	public static GameObject bubblePrefab;

	public static Vector3 bubbleOffset = new Vector3(0f, 0.15f, 0f);

	public static ConfigEntry<int> BubbleSize;

	public static ConfigEntry<int> BubbleSpeed;

	private readonly float cooldown = 0.1f;

	private readonly float margin = 0.1f;

	private float baseDrag;

	public GameObject bubble;

	public GameObject colliderObject;

	private float colliderScale = 1f;

	private float lastTouchLeft;

	private float lastTouchRight;

	private bool leftWasTouching;

	private bool rightWasTouching;

	private Rigidbody rb;

	public Vector3 targetPosition;

	private void Awake()
	{
		if (!Object.op_Implicit((Object)(object)bubblePrefab))
		{
			bubblePrefab = Plugin.AssetBundle.LoadAsset<GameObject>("BubbleP");
		}
		NetworkPropertyHandler instance = NetworkPropertyHandler.Instance;
		instance.OnPlayerModStatusChanged = (Action<NetPlayer, string, bool>)Delegate.Combine(instance.OnPlayerModStatusChanged, new Action<NetPlayer, string, bool>(OnPlayerModStatusChanged));
		VRRigCachePatches.OnRigCached = (Action<NetPlayer, VRRig>)Delegate.Combine(VRRigCachePatches.OnRigCached, new Action<NetPlayer, VRRig>(OnRigCached));
	}

	private void FixedUpdate()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		if (!Object.op_Implicit((Object)(object)rb))
		{
			rb = ((Collider)GTPlayer.Instance.bodyCollider).attachedRigidbody;
		}
		rb.AddForce(-Physics.gravity * rb.mass * GTPlayer.Instance.scale);
		bubble.transform.localScale = Vector3.one * GTPlayer.Instance.scale * 0.75f;
	}

	private void LateUpdate()
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)bubble != (Object)null))
		{
			return;
		}
		bubble.transform.position = ((Component)GTPlayer.Instance.headCollider).transform.position;
		Transform transform = bubble.transform;
		transform.position -= bubbleOffset * GTPlayer.Instance.scale;
		Vector3 position = GestureTracker.Instance.leftHand.transform.position;
		Vector3 position2 = GestureTracker.Instance.rightHand.transform.position;
		if (Touching(position))
		{
			if (!leftWasTouching && Time.time - lastTouchLeft > cooldown)
			{
				OnTouch(position, left: true);
				lastTouchLeft = Time.time;
			}
			leftWasTouching = true;
		}
		else
		{
			leftWasTouching = false;
		}
		if (Touching(position2))
		{
			if (!rightWasTouching && Time.time - lastTouchRight > cooldown)
			{
				OnTouch(position2, left: false);
				lastTouchRight = Time.time;
			}
			rightWasTouching = true;
		}
		else
		{
			rightWasTouching = false;
		}
	}

	protected override void OnEnable()
	{
		if (!MenuController.Instance.Built)
		{
			return;
		}
		base.OnEnable();
		try
		{
			ReloadConfiguration();
			bubble = Object.Instantiate<GameObject>(bubblePrefab);
			bubble.AddComponent<GorillaSurfaceOverride>().overrideIndex = 110;
			bubble.GetComponent<Collider>().enabled = false;
			rb = ((Collider)GTPlayer.Instance.bodyCollider).attachedRigidbody;
			baseDrag = rb.drag;
			rb.drag = 1f;
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	private void OnRigCached(NetPlayer player, VRRig rig)
	{
		if (rig != null)
		{
			GameObject gameObject = ((Component)rig).gameObject;
			if (gameObject != null)
			{
				((Component)(object)gameObject.GetComponent<BubbleMarker>())?.Obliterate();
			}
		}
	}

	private void OnPlayerModStatusChanged(NetPlayer player, string mod, bool enabled)
	{
		if (!(mod != DisplayName))
		{
			if (enabled)
			{
				((Component)player.Rig()).gameObject.GetOrAddComponent<BubbleMarker>();
			}
			else
			{
				Object.Destroy((Object)(object)((Component)player.Rig()).gameObject.GetComponent<BubbleMarker>());
			}
		}
	}

	private bool Touching(Vector3 position)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		float num = GTPlayer.Instance.scale * colliderScale;
		float num2 = Vector3.Distance(position, bubble.transform.position);
		float num3 = margin * GTPlayer.Instance.scale;
		if (num2 > num - num3)
		{
			return num2 < num + num3;
		}
		return false;
	}

	private void OnTouch(Vector3 position, bool left)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		Sounds.Play(110);
		position -= bubble.transform.position;
		GestureTracker.Instance.HapticPulse(left);
		GTPlayer.Instance.AddForce(((Vector3)(ref position)).normalized * GTPlayer.Instance.scale * (float)BubbleSpeed.Value / 5f);
	}

	protected override void Cleanup()
	{
		if (Object.op_Implicit((Object)(object)bubble))
		{
			Sounds.Play(84, 2f);
		}
		bubble?.Obliterate();
		if (Object.op_Implicit((Object)(object)rb))
		{
			rb.drag = baseDrag;
		}
	}

	protected override void ReloadConfiguration()
	{
		colliderScale = MathExtensions.Map(BubbleSize.Value, 0f, 10f, 0.45f, 0.65f);
	}

	public static void BindConfigEntries()
	{
		BubbleSize = Plugin.ConfigFile.Bind<int>(DisplayName, "bubble size", 5, "How far you have to reach to hit the bubble");
		BubbleSpeed = Plugin.ConfigFile.Bind<int>(DisplayName, "bubble speed", 5, "How fast the bubble moves when you push it");
	}

	public override string GetDisplayName()
	{
		return DisplayName;
	}

	public override string Tutorial()
	{
		return "Creates a bubble around you so you can float. Tap the side that you want to move towards to move.";
	}
}
