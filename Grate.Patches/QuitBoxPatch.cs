using GorillaNetworking;
using HarmonyLib;
using UnityEngine;

namespace Grate.Patches;

[HarmonyPatch(typeof(GorillaQuitBox))]
[HarmonyPatch(/*Could not decode attribute arguments.*/)]
internal class QuitBoxPatch
{
	private static bool Prefix()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		TeleportPatch.TeleportPlayer(new Vector3(-66.4845f, 11.7564f, -82.5688f), 0f);
		GameObject[] enableOnStartup = ((PhotonNetworkController)PhotonNetworkController.Instance).enableOnStartup;
		for (int i = 0; i < enableOnStartup.Length; i++)
		{
			enableOnStartup[i].SetActive(true);
		}
		enableOnStartup = ((PhotonNetworkController)PhotonNetworkController.Instance).disableOnStartup;
		for (int i = 0; i < enableOnStartup.Length; i++)
		{
			enableOnStartup[i].SetActive(false);
		}
		return !Plugin.WaWaGrazeDotCc;
	}
}
