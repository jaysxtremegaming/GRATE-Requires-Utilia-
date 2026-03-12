using System;
using Grate.Extensions;
using Grate.Modules.Physics;
using Grate.Tools;
using HarmonyLib;
using UnityEngine;

namespace Grate.Patches;

[HarmonyPatch(typeof(SizeManager))]
[HarmonyPatch(/*Could not decode attribute arguments.*/)]
public class SizeChangePatch
{
	private static void Postfix(ref SizeChanger __result, Transform t)
	{
		if (!Plugin.WaWaGrazeDotCc)
		{
			return;
		}
		try
		{
			if (Potions.active && (Object)(object)t == (Object)(object)((Component)GorillaTagger.Instance.offlineVRRig).transform)
			{
				__result = Potions.sizeChanger;
			}
			else if (Potions.ShowNetworkedSizes != null && Potions.ShowNetworkedSizes.Value)
			{
				VRRig componentInParent = ((Component)t).GetComponentInParent<VRRig>();
				if (componentInParent != null && componentInParent.ModuleEnabled(Potions.DisplayName))
				{
					Potions.TryGetSizeChangerForRig(componentInParent, out __result);
				}
			}
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}
}
