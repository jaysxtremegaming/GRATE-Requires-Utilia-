using System;
using System.Threading.Tasks;
using GorillaLocomotion;
using Grate.Modules.Physics;
using Grate.Tools;
using HarmonyLib;
using UnityEngine;

namespace Grate.Patches;

[HarmonyPatch(typeof(GTPlayer))]
[HarmonyPatch(/*Could not decode attribute arguments.*/)]
internal class TeleportPatch
{
	private static bool _isTeleporting;

	private static bool _rotate;

	private static Vector3 _teleportPosition;

	private static float _teleportRotation;

	private static bool _killVelocity;

	public static bool Prefix(GTPlayer __instance)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (_isTeleporting)
			{
				GTPlayer.Instance.locomotionEnabledLayers = LayerMask.op_Implicit(NoClip.layerMask);
				((Collider)GTPlayer.Instance.bodyCollider).isTrigger = true;
				((Collider)GTPlayer.Instance.headCollider).isTrigger = true;
				Rigidbody component = ((Component)__instance).GetComponent<Rigidbody>();
				if ((Object)(object)component != (Object)null)
				{
					Vector3 val = _teleportPosition - ((Component)__instance.bodyCollider).transform.position + ((Component)__instance).transform.position;
					if (_killVelocity)
					{
						component.velocity = Vector3.zero;
					}
					((Component)__instance).transform.position = val;
					if (_rotate)
					{
						((Component)__instance).transform.rotation = Quaternion.Euler(0f, _teleportRotation, 0f);
					}
					Traverse.Create((object)__instance).Field("lastLeftHandPosition").SetValue((object)((Component)__instance.leftHand.handFollower).transform.position);
					Traverse.Create((object)__instance).Field("lastRightHandPosition").SetValue((object)((Component)__instance.rightHand.handFollower).transform.position);
					Traverse.Create((object)__instance).Field("lastPosition").SetValue((object)val);
					Traverse.Create((object)__instance).Field("lastOpenHeadPosition").SetValue((object)((Component)__instance.headCollider).transform.position);
					((Component)GorillaTagger.Instance.offlineVRRig).transform.position = val;
				}
				((Collider)GTPlayer.Instance.headCollider).isTrigger = NoClip.Instance.baseHeadIsTrigger;
				((Collider)GTPlayer.Instance.bodyCollider).isTrigger = NoClip.Instance.baseBodyIsTrigger;
				Task.Delay(250).ContinueWith(delegate
				{
					//IL_000a: Unknown result type (might be due to invalid IL or missing references)
					//IL_000f: Unknown result type (might be due to invalid IL or missing references)
					GTPlayer.Instance.locomotionEnabledLayers = NoClip.Instance.baseMask;
				});
				_isTeleporting = false;
				return true;
			}
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
		return true;
	}

	internal static void TeleportPlayer(Vector3 destinationPosition, float destinationRotation, bool killVelocity = true)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (!_isTeleporting)
		{
			_killVelocity = killVelocity;
			_teleportPosition = destinationPosition;
			_teleportRotation = destinationRotation;
			_isTeleporting = true;
			_rotate = true;
		}
	}

	internal static void TeleportPlayer(Vector3 destinationPosition, bool killVelocity = true)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (!_isTeleporting)
		{
			_killVelocity = killVelocity;
			_teleportPosition = destinationPosition;
			_isTeleporting = true;
			_rotate = false;
		}
	}
}
