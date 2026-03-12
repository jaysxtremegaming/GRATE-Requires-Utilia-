using System;
using BepInEx.Configuration;
using GorillaLocomotion;
using Grate.GUI;
using Grate.Gestures;
using UnityEngine;

namespace Grate.Modules.Movement;

public class Airplane : GrateModule
{
	public static readonly string DisplayName = "Airplane";

	public static ConfigEntry<int> Speed;

	public static ConfigEntry<string> SteerWith;

	private readonly float acceleration = 0.1f;

	private float speedScale = 10f;

	protected override void OnEnable()
	{
		if (MenuController.Instance.Built)
		{
			base.OnEnable();
			ReloadConfiguration();
			GestureTracker instance = GestureTracker.Instance;
			instance.OnGlide = (Action<Vector3>)Delegate.Combine(instance.OnGlide, new Action<Vector3>(OnGlide));
			((Component)Plugin.MenuController).GetComponent<Helicopter>().button.AddBlocker(ButtonController.Blocker.MOD_INCOMPAT);
		}
	}

	private void OnGlide(Vector3 direction)
	{
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		if (!((Behaviour)this).enabled)
		{
			return;
		}
		GestureTracker instance = GestureTracker.Instance;
		if (instance.leftTrigger.pressed || instance.rightTrigger.pressed || instance.leftGrip.pressed || instance.rightGrip.pressed)
		{
			return;
		}
		GTPlayer instance2 = GTPlayer.Instance;
		if (!instance2.rightHand.wasColliding && !instance2.leftHand.wasColliding)
		{
			if (SteerWith.Value == "head")
			{
				direction = ((Component)instance2.headCollider).transform.forward;
			}
			Rigidbody attachedRigidbody = ((Collider)instance2.bodyCollider).attachedRigidbody;
			Vector3 val = direction * instance2.scale * speedScale;
			attachedRigidbody.velocity = Vector3.Lerp(attachedRigidbody.velocity, val, acceleration);
		}
	}

	protected override void Cleanup()
	{
		if (MenuController.Instance.Built && Object.op_Implicit((Object)(object)GestureTracker.Instance))
		{
			GestureTracker instance = GestureTracker.Instance;
			instance.OnGlide = (Action<Vector3>)Delegate.Remove(instance.OnGlide, new Action<Vector3>(OnGlide));
			((Component)Plugin.MenuController).GetComponent<Helicopter>().button.RemoveBlocker(ButtonController.Blocker.MOD_INCOMPAT);
		}
	}

	protected override void ReloadConfiguration()
	{
		speedScale = Speed.Value * 2;
	}

	public static void BindConfigEntries()
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		Speed = Plugin.ConfigFile.Bind<int>(DisplayName, "speed", 5, "How fast you fly");
		SteerWith = Plugin.ConfigFile.Bind<string>(DisplayName, "steer with", "wrists", new ConfigDescription("Which part of your body you use to steer", (AcceptableValueBase)(object)new AcceptableValueList<string>(new string[2] { "wrists", "head" }), Array.Empty<object>()));
	}

	public override string GetDisplayName()
	{
		return DisplayName;
	}

	public override string Tutorial()
	{
		return "- To fly, do a T-pose (spread your arms out like wings on a plane). \n- To fly up, point your thumbs up. \n- To fly down, point your thumbs down.";
	}
}
