using System;
using BepInEx.Configuration;
using GorillaLocomotion;
using Grate.GUI;
using Grate.Gestures;
using UnityEngine;

namespace Grate.Modules.Movement;

public class DoubleJump : GrateModule
{
	public static readonly string DisplayName = "Double Jump";

	public static bool canDoubleJump = true;

	public static ConfigEntry<string> JumpForce;

	private GTPlayer _player;

	private Rigidbody _rigidbody;

	private Vector3 direction;

	public static bool primaryPressed => GestureTracker.Instance.rightPrimary.pressed;

	private void FixedUpdate()
	{
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		if (_player.rightHand.wasColliding || _player.leftHand.wasColliding)
		{
			canDoubleJump = true;
		}
		if (canDoubleJump && primaryPressed && !_player.rightHand.wasColliding && !_player.leftHand.wasColliding)
		{
			direction = (((Component)_player.headCollider).transform.forward + Vector3.up) / 2f;
			_rigidbody.velocity = new Vector3(direction.x, direction.y, direction.z) * _player.maxJumpSpeed * _player.scale * GetJumpForce(JumpForce.Value);
			canDoubleJump = false;
		}
	}

	protected override void OnEnable()
	{
		if (MenuController.Instance.Built)
		{
			base.OnEnable();
			_player = GTPlayer.Instance;
			_rigidbody = ((Collider)_player.bodyCollider).attachedRigidbody;
		}
	}

	private float GetJumpForce(string jumpforce)
	{
		return jumpforce switch
		{
			"Normal" => 2f, 
			"Medium" => 2.5f, 
			"High" => 2.8f, 
			"Super Jump" => 3.3f, 
			_ => 2f, 
		};
	}

	public static void BindConfigEntries()
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		JumpForce = Plugin.ConfigFile.Bind<string>(DisplayName, "Jump Force", "Normal", new ConfigDescription("How high you jump", (AcceptableValueBase)(object)new AcceptableValueList<string>(new string[4] { "Normal", "Medium", "High", "Super Jump" }), Array.Empty<object>()));
	}

	protected override void Cleanup()
	{
	}

	public override string GetDisplayName()
	{
		return DisplayName;
	}

	public override string Tutorial()
	{
		return "Press [A / B] on your right controller to do a double jump in the air.";
	}
}
