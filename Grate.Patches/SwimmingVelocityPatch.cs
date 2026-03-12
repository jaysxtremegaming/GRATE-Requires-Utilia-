using System;
using GorillaLocomotion;
using Grate.Modules;
using Grate.Tools;
using HarmonyLib;
using UnityEngine;

namespace Grate.Patches;

[HarmonyPatch(typeof(GTPlayer))]
[HarmonyPatch(/*Could not decode attribute arguments.*/)]
internal class SwimmingVelocityPatch
{
	private static void Postfix(ref Vector3 swimmingVelocityChange)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (SpeedBoost.active)
			{
				swimmingVelocityChange *= SpeedBoost.scale;
			}
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}
}
