using System;
using Grate.Tools;
using HarmonyLib;
using UnityEngine;

namespace Grate.Patches;

[HarmonyPatch(typeof(VRRig))]
[HarmonyPatch(/*Could not decode attribute arguments.*/)]
public class VRRigLateUpdatePatch
{
	private static void Postfix(VRRig __instance, ref AudioSource ___voiceAudio)
	{
		if (!Plugin.WaWaGrazeDotCc || !Object.op_Implicit((Object)(object)___voiceAudio))
		{
			return;
		}
		try
		{
			___voiceAudio.pitch = Mathf.Clamp(___voiceAudio.pitch, 0.8f, 1.2f);
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}
}
