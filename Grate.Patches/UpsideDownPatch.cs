using System.Collections.Generic;
using System.Linq;
using GorillaLocomotion;
using HarmonyLib;
using UnityEngine;

namespace Grate.Patches;

[HarmonyPatch(typeof(VRRig), "PostTick")]
public class UpsideDownPatch
{
	public static Dictionary<VRRig?, bool> AffectedRigs = new Dictionary<VRRig, bool>();

	private static void Postfix(VRRig __instance)
	{
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		if (AffectedRigs.Keys.Contains(__instance))
		{
			if (!AffectedRigs[__instance])
			{
				((Component)__instance).transform.Rotate(180f, 180f, 0f);
				AffectedRigs[__instance] = true;
			}
			if (__instance.isLocal)
			{
				__instance.leftHand.MapMine(__instance.scaleFactor, __instance.playerOffsetTransform);
				__instance.rightHand.MapMine(__instance.scaleFactor, __instance.playerOffsetTransform);
				__instance.head.MapMine(__instance.scaleFactor, __instance.playerOffsetTransform);
				__instance.head.rigTarget.rotation = ((Component)GTPlayer.Instance.headCollider).transform.rotation;
			}
		}
	}
}
