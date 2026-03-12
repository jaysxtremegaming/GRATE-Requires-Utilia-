using System.Reflection;
using BepInEx.Configuration;
using GorillaLocomotion;
using Grate.GUI;
using Grate.Modules.Physics;
using UnityEngine;

namespace Grate.Modules.Movement;

public class Wallrun : GrateModule
{
	public static readonly string DisplayName = "Wall Run";

	public static ConfigEntry<int> Power;

	private Vector3 baseGravity;

	private RaycastHit hit;

	private void Awake()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		baseGravity = Physics.gravity;
	}

	protected void FixedUpdate()
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		GTPlayer instance = GTPlayer.Instance;
		if (instance.leftHand.wasColliding || instance.rightHand.wasColliding)
		{
			FieldInfo field = typeof(GTPlayer).GetField("lastHitInfoHand", BindingFlags.Instance | BindingFlags.NonPublic);
			hit = (RaycastHit)field.GetValue(instance);
			Physics.gravity = ((RaycastHit)(ref hit)).normal * (0f - ((Vector3)(ref baseGravity)).magnitude) * GravScale();
		}
		else if (Vector3.Distance(((Component)instance.bodyCollider).transform.position, ((RaycastHit)(ref hit)).point) > 2f * GTPlayer.Instance.scale)
		{
			Cleanup();
		}
	}

	protected override void OnEnable()
	{
		if (MenuController.Instance.Built)
		{
			base.OnEnable();
		}
	}

	public float GravScale()
	{
		if (!LowGravity.Instance.active)
		{
			return (float)Power.Value * 0.15f + 0.25f;
		}
		return LowGravity.Instance.gravityScale;
	}

	public static void BindConfigEntries()
	{
		Power = Plugin.ConfigFile.Bind<int>(DisplayName, "power", 5, "Wall Run Strength \n5 means it will have normal gravity power in the direction of the last hit wall");
	}

	protected override void Cleanup()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		Physics.gravity = baseGravity;
	}

	public override string GetDisplayName()
	{
		return DisplayName;
	}

	public override string Tutorial()
	{
		return "Effect: Allows you to walk on any surface, no matter the angle.";
	}
}
