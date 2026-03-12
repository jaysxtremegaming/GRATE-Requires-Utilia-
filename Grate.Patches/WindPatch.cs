using Grate.Modules.Misc;
using HarmonyLib;
using UnityEngine;

namespace Grate.Patches;

[HarmonyPatch(typeof(ForceVolume))]
[HarmonyPatch(/*Could not decode attribute arguments.*/)]
internal class WindPatch
{
	private static bool Prefix(ForceVolume __instance)
	{
		if (DisableWind.Enabled)
		{
			if ((Object)(object)__instance.audioSource != (Object)null)
			{
				((Behaviour)__instance.audioSource).enabled = false;
			}
			Collider value = Traverse.Create((object)__instance).Field<Collider>("volume").Value;
			if ((Object)(object)value != (Object)null)
			{
				value.enabled = false;
			}
			return false;
		}
		Collider value2 = Traverse.Create((object)__instance).Field<Collider>("volume").Value;
		if ((Object)(object)value2 != (Object)null)
		{
			value2.enabled = true;
		}
		return true;
	}
}
