using System;
using GorillaLocomotion;
using Grate.Modules.Physics;
using Grate.Tools;
using HarmonyLib;
using UnityEngine;

namespace Grate.Patches;

[HarmonyPatch(typeof(GTPlayer))]
[HarmonyPatch(/*Could not decode attribute arguments.*/)]
public class SlidePatch
{
	private static void Postfix(GTPlayer __instance, ref float __result)
	{
		try
		{
			if (Object.op_Implicit((Object)(object)SlipperyHands.Instance))
			{
				__result = (((Behaviour)SlipperyHands.Instance).enabled ? 1f : __result);
			}
			if (Object.op_Implicit((Object)(object)NoSlip.Instance))
			{
				__result = (((Behaviour)NoSlip.Instance).enabled ? 0f : __result);
			}
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}
}
