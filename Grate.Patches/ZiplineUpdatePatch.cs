using System;
using GorillaLocomotion.Climbing;
using GorillaLocomotion.Gameplay;
using Grate.Modules.Movement;
using Grate.Tools;
using HarmonyLib;
using UnityEngine;

namespace Grate.Patches;

[HarmonyPatch(typeof(GorillaZipline))]
[HarmonyPatch(/*Could not decode attribute arguments.*/)]
public class ZiplineUpdatePatch
{
	private static void Postfix(GorillaZipline __instance, BezierSpline ___spline, float ___currentT, GorillaHandClimber ___currentClimber)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		if (!Plugin.WaWaGrazeDotCc)
		{
			return;
		}
		try
		{
			Rockets instance = Rockets.Instance;
			if (Object.op_Implicit((Object)(object)instance) && ((Behaviour)instance).enabled && Object.op_Implicit((Object)(object)___currentClimber))
			{
				Vector3 currentDirection = __instance.GetCurrentDirection();
				Vector3 val = instance.AddedVelocity();
				Traverse obj = Traverse.Create((object)__instance).Property("currentSpeed", (object[])null);
				float num = Vector3.Dot(currentDirection, val) * Time.deltaTime * ((Vector3)(ref val)).magnitude * 1000f;
				obj.SetValue((object)(obj.GetValue<float>() + num));
			}
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}
}
