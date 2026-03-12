using System.Linq;
using GorillaLocomotion;
using HarmonyLib;
using UnityEngine;

namespace Grate.Patches;

[HarmonyPatch(typeof(GTPlayer), "LateUpdate")]
public class UpsideDownPatchGtPlayer
{
	private static void Postfix(GTPlayer __instance)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		if (UpsideDownPatch.AffectedRigs.Keys.Contains(VRRig.LocalRig))
		{
			((Component)__instance.bodyCollider).transform.Rotate(0f, 0f, 180f);
			((Component)__instance.bodyCollider).transform.localPosition = new Vector3(0f, -0.3f, 0f);
		}
	}
}
