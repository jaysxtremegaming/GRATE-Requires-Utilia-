using GT_CustomMapSupportRuntime;
using GorillaLocomotion;
using Grate.GUI;
using UnityEngine;

namespace Grate.Modules.Misc;

public class ReturnToVS : GrateModule
{
	protected override void OnEnable()
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		if (MenuController.Instance.Built)
		{
			base.OnEnable();
			if ((Object)(object)Object.FindObjectOfType<AccessDoorPlaceholder>() != (Object)null)
			{
				Transform transform = ((Component)Object.FindObjectOfType<AccessDoorPlaceholder>()).transform;
				GTPlayer.Instance.TeleportTo(transform.position + new Vector3(0f, 0.1f, 0f), transform.rotation, false, false);
			}
			((Behaviour)this).enabled = false;
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	public override string GetDisplayName()
	{
		return "Return To VStump";
	}

	public override string Tutorial()
	{
		return "Press to go back to the virtual stump \n Only Works when in Map (duh)";
	}

	protected override void Cleanup()
	{
	}
}
