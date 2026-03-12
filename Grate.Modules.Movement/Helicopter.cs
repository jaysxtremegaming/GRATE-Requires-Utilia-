using System;
using BepInEx.Configuration;
using GorillaLocomotion;
using Grate.GUI;
using Grate.Gestures;
using UnityEngine;

namespace Grate.Modules.Movement;

public class Helicopter : GrateModule
{
	public static readonly string DisplayName = "Helicopter";

	public static ConfigEntry<int> Speed;

	public static ConfigEntry<string> Mode;

	public static ConfigEntry<string> spin;

	protected override void OnEnable()
	{
		if (MenuController.Instance.Built)
		{
			base.OnEnable();
			GestureTracker instance = GestureTracker.Instance;
			instance.OnGlide = (Action<Vector3>)Delegate.Combine(instance.OnGlide, new Action<Vector3>(OnGlide));
			((Component)Plugin.MenuController).GetComponent<Airplane>().button.AddBlocker(ButtonController.Blocker.MOD_INCOMPAT);
		}
	}

	private void OnGlide(Vector3 direction)
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		if (!((Behaviour)this).enabled)
		{
			return;
		}
		GestureTracker instance = GestureTracker.Instance;
		if (!instance.leftTrigger.pressed && !instance.rightTrigger.pressed && !instance.leftGrip.pressed && !instance.rightGrip.pressed)
		{
			GTPlayer instance2 = GTPlayer.Instance;
			float y = ((Component)instance2.headCollider).transform.forward.y;
			if (!instance2.LeftHand.wasColliding && !instance2.rightHand.wasColliding && Threshold(15f, y))
			{
				((Collider)instance2.bodyCollider).attachedRigidbody.velocity = new Vector3(0f, (float)Speed.Value * GTPlayer.Instance.scale * Towards(y), 0f);
				instance2.Turn((float)Speed.Value * Time.fixedDeltaTime * 20f * Towards(y) * (float)((spin.Value == "normal") ? 1 : (-1)));
			}
		}
	}

	public static bool Threshold(float angle, float direction)
	{
		if (direction > angle || direction < 0f - angle)
		{
			return false;
		}
		return true;
	}

	public static float Towards(float direction)
	{
		if (Mode.Value == "snappy")
		{
			return (!(direction < 0f)) ? 1 : (-1);
		}
		return direction * 2f;
	}

	protected override void Cleanup()
	{
		if (MenuController.Instance.Built && Object.op_Implicit((Object)(object)GestureTracker.Instance))
		{
			GestureTracker instance = GestureTracker.Instance;
			instance.OnGlide = (Action<Vector3>)Delegate.Remove(instance.OnGlide, new Action<Vector3>(OnGlide));
			((Component)Plugin.MenuController).GetComponent<Airplane>().button.RemoveBlocker(ButtonController.Blocker.MOD_INCOMPAT);
		}
	}

	public static void BindConfigEntries()
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Expected O, but got Unknown
		Speed = Plugin.ConfigFile.Bind<int>(DisplayName, "speed", 5, "How fast you spin");
		Mode = Plugin.ConfigFile.Bind<string>(DisplayName, "mode", "snappy", new ConfigDescription("The way your head controls the speed", (AcceptableValueBase)(object)new AcceptableValueList<string>(new string[2] { "snappy", "smooth" }), Array.Empty<object>()));
		spin = Plugin.ConfigFile.Bind<string>(DisplayName, "direction", "normal", new ConfigDescription("The direction of the spin", (AcceptableValueBase)(object)new AcceptableValueList<string>(new string[2] { "normal", "reverse" }), Array.Empty<object>()));
	}

	public override string GetDisplayName()
	{
		return DisplayName;
	}

	public override string Tutorial()
	{
		return "- WARNING: CAN CAUSE MOTION SICKNESS EASILY \n -To spin, do a T-pose (spread your arms out like wings on a Helicopter). \n- Look up to fly up.\n- Look down to fly down.";
	}
}
