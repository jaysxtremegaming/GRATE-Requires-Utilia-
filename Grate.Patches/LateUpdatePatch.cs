using System;
using GorillaLocomotion;
using HarmonyLib;
using UnityEngine;

namespace Grate.Patches;

[HarmonyPatch(typeof(GTPlayer))]
[HarmonyPatch(/*Could not decode attribute arguments.*/)]
public class LateUpdatePatch
{
	public static Action<GTPlayer> OnLateUpdate;

	private static void Postfix(GTPlayer __instance)
	{
		try
		{
			OnLateUpdate?.Invoke(__instance);
		}
		catch
		{
		}
		Camera.main.farClipPlane = 8500f;
		Camera.main.clearFlags = (CameraClearFlags)1;
	}
}
