using BepInEx.Configuration;
using GorillaLocomotion;
using Grate.Extensions;
using Grate.GUI;
using UnityEngine;

namespace Grate.Modules.Movement;

public class HandFly : GrateModule
{
	public static string DisplayName = "Hand Fly";

	private static ConfigEntry<int>? Speed;

	private LocalGorillaVelocityTracker? left;

	private LocalGorillaVelocityTracker? right;

	private float SpeedScale => (float)Speed.Value * 2.5f + 10f;

	private void FixedUpdate()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		if (((ControllerInputPoller)ControllerInputPoller.instance).leftControllerIndexFloat > 0.5f)
		{
			Rigidbody rigidbody = GorillaTagger.Instance.rigidbody;
			rigidbody.velocity -= right.GetVelocity() / SpeedScale * GTPlayer.Instance.scale;
		}
		if (((ControllerInputPoller)ControllerInputPoller.instance).rightControllerIndexFloat > 0.5f)
		{
			Rigidbody rigidbody2 = GorillaTagger.Instance.rigidbody;
			rigidbody2.velocity -= left.GetVelocity() / SpeedScale * GTPlayer.Instance.scale;
		}
	}

	protected override void OnEnable()
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		if (MenuController.Instance.Built && ((Behaviour)this).enabled)
		{
			MenuController? menuController = Plugin.MenuController;
			if (menuController != null)
			{
				((Component)menuController).GetComponent<Fly>().button.AddBlocker(ButtonController.Blocker.MOD_INCOMPAT);
			}
			right = ComponentUtils.AddComponent<LocalGorillaVelocityTracker>((Component)(object)GTPlayer.Instance.LeftHand.controllerTransform);
			left = ComponentUtils.AddComponent<LocalGorillaVelocityTracker>((Component)(object)GTPlayer.Instance.RightHand.controllerTransform);
			ReloadConfiguration();
			base.OnEnable();
		}
	}

	public override string GetDisplayName()
	{
		return DisplayName;
	}

	public override string Tutorial()
	{
		return "-To fly, press 'Trigger' to Throw yourself,\nboth hands for more speed";
	}

	protected override void Cleanup()
	{
		MenuController? menuController = Plugin.MenuController;
		if (menuController != null)
		{
			((Component)menuController).GetComponent<Fly>().button.RemoveBlocker(ButtonController.Blocker.MOD_INCOMPAT);
		}
		if ((Object)(object)right != (Object)null)
		{
			((Component)(object)right).Obliterate();
		}
		if ((Object)(object)left != (Object)null)
		{
			((Component)(object)left).Obliterate();
		}
	}

	public static void BindConfigEntries()
	{
		Speed = Plugin.ConfigFile.Bind<int>(DisplayName, "speed", 5, "How fast you fly");
	}
}
