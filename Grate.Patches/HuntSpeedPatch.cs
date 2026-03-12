using System;
using Grate.Modules;
using Grate.Tools;
using HarmonyLib;

namespace Grate.Patches;

[HarmonyPatch(typeof(GorillaHuntManager))]
[HarmonyPatch(/*Could not decode attribute arguments.*/)]
internal class HuntSpeedPatch
{
	private static void Postfix(GorillaHuntManager __instance, ref float[] __result)
	{
		try
		{
			if (SpeedBoost.active)
			{
				for (int i = 0; i < __result.Length; i++)
				{
					__result[i] *= SpeedBoost.scale;
				}
			}
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}
}
