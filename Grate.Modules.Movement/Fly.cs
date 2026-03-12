using BepInEx.Configuration;
using GorillaLocomotion;
using Grate.Extensions;
using Grate.GUI;
using Grate.Gestures;
using UnityEngine;

namespace Grate.Modules.Movement;

public class Fly : GrateModule
{
	public static readonly string DisplayName = "Fly";

	public static ConfigEntry<int> Speed;

	public static ConfigEntry<int> Acceleration;

	private float speedScale = 10f;

	private float acceleration = 0.01f;

	private bool _toggle = true;

	private Vector2 xz;

	private float y;

	private bool pressed;

	private void FixedUpdate()
	{
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		bool flag = GestureTracker.Instance.leftPrimary.pressed || GestureTracker.Instance.rightPrimary.pressed;
		if (flag && !pressed)
		{
			_toggle = !_toggle;
		}
		pressed = flag;
		if (_toggle)
		{
			Rigidbody attachedRigidbody = ((Collider)GTPlayer.Instance.bodyCollider).attachedRigidbody;
			if (GrateModule.enabledModules.ContainsKey(Bubble.DisplayName) && !GrateModule.enabledModules[Bubble.DisplayName])
			{
				attachedRigidbody.AddForce(-Physics.gravity * attachedRigidbody.mass * GTPlayer.Instance.scale);
			}
			xz = GestureTracker.Instance.leftStickAxis.GetValue();
			y = GestureTracker.Instance.rightStickAxis.GetValue().y;
			Vector3 val = default(Vector3);
			((Vector3)(ref val))._002Ector(xz.x, y, xz.y);
			Vector3 forward = ((Component)GTPlayer.Instance.bodyCollider).transform.forward;
			forward.y = 0f;
			Vector3 right = ((Component)GTPlayer.Instance.bodyCollider).transform.right;
			right.y = 0f;
			Vector3 val2 = val.x * right + y * Vector3.up + val.z * forward;
			val2 *= GTPlayer.Instance.scale * speedScale;
			attachedRigidbody.velocity = Vector3.Lerp(attachedRigidbody.velocity, val2, acceleration);
		}
	}

	protected override void OnEnable()
	{
		if (MenuController.Instance.Built)
		{
			base.OnEnable();
			MenuController? menuController = Plugin.MenuController;
			if (menuController != null)
			{
				((Component)menuController).GetComponent<HandFly>().button.AddBlocker(ButtonController.Blocker.MOD_INCOMPAT);
			}
			ReloadConfiguration();
		}
	}

	public override string GetDisplayName()
	{
		return "Fly";
	}

	public override string Tutorial()
	{
		return "Use left stick to fly horizontally, and right stick to fly vertically.\nPrees Either Primary Buttons to Toggle while mod is enabled";
	}

	protected override void ReloadConfiguration()
	{
		speedScale = Speed.Value * 2;
		acceleration = Acceleration.Value;
		if (acceleration == 10f)
		{
			acceleration = 1f;
		}
		else
		{
			acceleration = MathExtensions.Map(Acceleration.Value, 0f, 10f, 0.0075f, 0.25f);
		}
	}

	public static void BindConfigEntries()
	{
		Speed = Plugin.ConfigFile.Bind<int>(DisplayName, "speed", 5, "How fast you fly");
		Acceleration = Plugin.ConfigFile.Bind<int>(DisplayName, "acceleration", 5, "How fast you accelerate");
	}

	protected override void Cleanup()
	{
		MenuController? menuController = Plugin.MenuController;
		if (menuController != null)
		{
			((Component)menuController).GetComponent<HandFly>().button.RemoveBlocker(ButtonController.Blocker.MOD_INCOMPAT);
		}
	}
}
