using System;
using Grate.Modules;
using Grate.Tools;
using HarmonyLib;

namespace Grate.Patches;

[HarmonyPatch(typeof(GorillaPaintbrawlManager))]
[HarmonyPatch(/*Could not decode attribute arguments.*/)]
internal class BattleSpeedPatch
{
	private static void Postfix(GorillaPaintbrawlManager __instance, ref float[] __result)
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
