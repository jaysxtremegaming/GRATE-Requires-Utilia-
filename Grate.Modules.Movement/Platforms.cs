using System;
using BepInEx.Configuration;
using GorillaLocomotion;
using GorillaLocomotion.Climbing;
using Grate.Extensions;
using Grate.GUI;
using Grate.Gestures;
using Grate.Tools;
using UnityEngine;
using UnityEngine.XR;

namespace Grate.Modules.Movement;

public class Platforms : GrateModule
{
	public static readonly string DisplayName = "Platforms";

	public static GameObject platformPrefab;

	public static GorillaHandClimber LeftC;

	public static GorillaHandClimber RightC;

	private static Vector3 lastPositionL = Vector3.zero;

	private static Vector3 lastPositionR = Vector3.zero;

	private static Vector3 lastPositionHead = Vector3.zero;

	private static bool lHappen;

	private static bool rHappen;

	private static bool isVelocity;

	public static ConfigEntry<bool> Sticky;

	public static ConfigEntry<int> Scale;

	public static ConfigEntry<string> Input;

	public static ConfigEntry<string> Model;

	private InputTracker? inputL;

	private InputTracker? inputR;

	public Platform left;

	public Platform right;

	public Platform main;

	private void Awake()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Invalid comparison between Unknown and I4
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Invalid comparison between Unknown and I4
		if (!Object.op_Implicit((Object)(object)platformPrefab))
		{
			platformPrefab = Plugin.AssetBundle.LoadAsset<GameObject>("Bark Platform");
		}
		GorillaHandClimber[] array = Resources.FindObjectsOfTypeAll<GorillaHandClimber>();
		foreach (GorillaHandClimber val in array)
		{
			if ((int)val.xrNode == 4)
			{
				LeftC = val;
			}
			if ((int)val.xrNode == 5)
			{
				RightC = val;
			}
		}
	}

	private void FixedUpdate()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		if (!isVelocity)
		{
			return;
		}
		float num = 0.05f;
		Vector3 val = ((Component)GorillaTagger.Instance.headCollider).transform.position - lastPositionHead;
		Vector3 val2 = GorillaTagger.Instance.leftHandTransform.position - lastPositionL;
		Vector3 val3 = GorillaTagger.Instance.rightHandTransform.position - lastPositionR;
		bool num2 = Vector3.Dot(((Vector3)(ref val)).normalized, ((Vector3)(ref val2)).normalized) > 0.4f;
		bool flag = Vector3.Dot(((Vector3)(ref val)).normalized, ((Vector3)(ref val3)).normalized) > 0.4f;
		if (!num2)
		{
			if (GorillaTagger.Instance.leftHandTransform.position.y + num <= lastPositionL.y)
			{
				if (!lHappen)
				{
					lHappen = true;
					OnActivate(inputL);
				}
			}
			else if (lHappen)
			{
				lHappen = false;
				OnDeactivate(inputL);
			}
		}
		if (!flag)
		{
			if (GorillaTagger.Instance.rightHandTransform.position.y + num <= lastPositionR.y)
			{
				if (!rHappen)
				{
					rHappen = true;
					OnActivate(inputR);
				}
			}
			else if (rHappen)
			{
				rHappen = false;
				OnDeactivate(inputR);
			}
		}
		lastPositionL = GorillaTagger.Instance.leftHandTransform.position;
		lastPositionR = GorillaTagger.Instance.rightHandTransform.position;
		lastPositionHead = ((Component)GorillaTagger.Instance.headCollider).transform.position;
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
			left = CreatePlatform(isLeft: true);
			right = CreatePlatform(isLeft: false);
			ReloadConfiguration();
			((Component)Plugin.MenuController).GetComponent<Frozone>().button.AddBlocker(ButtonController.Blocker.MOD_INCOMPAT);
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	public Platform CreatePlatform(bool isLeft)
	{
		Platform platform = Object.Instantiate<GameObject>(platformPrefab).AddComponent<Platform>();
		platform.Initialize(isLeft);
		return platform;
	}

	public void OnActivate(InputTracker? tracker)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		if (((Behaviour)this).enabled)
		{
			bool flag = (int)tracker.node == 4;
			main = (flag ? left : right);
			Platform platform = ((!flag) ? left : right);
			main.Activate();
			if (Sticky.Value)
			{
				((Collider)GTPlayer.Instance.bodyCollider).attachedRigidbody.velocity = Vector3.zero;
				platform.Deactivate();
			}
		}
	}

	public void OnDeactivate(InputTracker? tracker)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		(((int)tracker.node == 4) ? left : right).Deactivate();
	}

	protected override void Cleanup()
	{
		if ((Object)(object)left != (Object)null)
		{
			GTPlayer.Instance.EndClimbing(LeftC, false, false);
			((Component)left).gameObject?.Obliterate();
		}
		if ((Object)(object)right != (Object)null)
		{
			GTPlayer.Instance.EndClimbing(RightC, false, false);
			((Component)right).gameObject?.Obliterate();
		}
		Unsub();
		((Component)Plugin.MenuController).GetComponent<Frozone>().button.RemoveBlocker(ButtonController.Blocker.MOD_INCOMPAT);
	}

	protected override void ReloadConfiguration()
	{
		left.Model = Model.Value;
		right.Model = Model.Value;
		left.Sticky = Sticky.Value;
		right.Sticky = Sticky.Value;
		float scale = MathExtensions.Map(Scale.Value, 0f, 10f, 0.5f, 1.5f);
		left.Scale = scale;
		right.Scale = scale;
		Unsub();
		if (Input.Value != "velocity")
		{
			inputL = GestureTracker.Instance.GetInputTracker(Input.Value, (XRNode)4);
			InputTracker? inputTracker = inputL;
			inputTracker.OnPressed = (Action<InputTracker>)Delegate.Combine(inputTracker.OnPressed, new Action<InputTracker>(OnActivate));
			InputTracker? inputTracker2 = inputL;
			inputTracker2.OnReleased = (Action<InputTracker>)Delegate.Combine(inputTracker2.OnReleased, new Action<InputTracker>(OnDeactivate));
			inputR = GestureTracker.Instance.GetInputTracker(Input.Value, (XRNode)5);
			InputTracker? inputTracker3 = inputR;
			inputTracker3.OnPressed = (Action<InputTracker>)Delegate.Combine(inputTracker3.OnPressed, new Action<InputTracker>(OnActivate));
			InputTracker? inputTracker4 = inputR;
			inputTracker4.OnReleased = (Action<InputTracker>)Delegate.Combine(inputTracker4.OnReleased, new Action<InputTracker>(OnDeactivate));
			isVelocity = false;
		}
		else
		{
			isVelocity = true;
		}
	}

	private void Unsub()
	{
		if (inputL != null)
		{
			InputTracker? inputTracker = inputL;
			inputTracker.OnPressed = (Action<InputTracker>)Delegate.Remove(inputTracker.OnPressed, new Action<InputTracker>(OnActivate));
			InputTracker? inputTracker2 = inputL;
			inputTracker2.OnReleased = (Action<InputTracker>)Delegate.Remove(inputTracker2.OnReleased, new Action<InputTracker>(OnDeactivate));
		}
		if (inputR != null)
		{
			InputTracker? inputTracker3 = inputR;
			inputTracker3.OnPressed = (Action<InputTracker>)Delegate.Remove(inputTracker3.OnPressed, new Action<InputTracker>(OnActivate));
			InputTracker? inputTracker4 = inputR;
			inputTracker4.OnReleased = (Action<InputTracker>)Delegate.Remove(inputTracker4.OnReleased, new Action<InputTracker>(OnDeactivate));
		}
	}

	public static void BindConfigEntries()
	{
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Expected O, but got Unknown
		try
		{
			Sticky = Plugin.ConfigFile.Bind<bool>(DisplayName, "sticky", false, "Whether or not your hands stick to the platforms");
			Scale = Plugin.ConfigFile.Bind<int>(DisplayName, "size", 5, "The size of the platforms");
			Input = Plugin.ConfigFile.Bind<string>(DisplayName, "input", "grip", new ConfigDescription("Which button you press to activate the platform", (AcceptableValueBase)(object)new AcceptableValueList<string>(new string[6] { "grip", "trigger", "stick", "a/x", "b/y", "velocity" }), Array.Empty<object>()));
			Model = Plugin.ConfigFile.Bind<string>(DisplayName, "model", "cloud", new ConfigDescription("Which button you press to activate the platform", (AcceptableValueBase)(object)new AcceptableValueList<string>(new string[5] { "cloud", "storm cloud", "doug", "ice", "invisible" }), Array.Empty<object>()));
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	public override string GetDisplayName()
	{
		return "Platforms";
	}

	public override string Tutorial()
	{
		return "Press [" + Input.Value + "] to spawn a platform you can stand on. Release [" + Input.Value + "] to disable it.";
	}
}
