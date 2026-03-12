using GorillaNetworking;
using Grate.Modules.Physics;
using Grate.Tools;
using HarmonyLib;
using UnityEngine;

namespace Grate.Patches;

internal class TriggerBoxPatches
{
	[HarmonyPatch(typeof(GorillaGeoHideShowTrigger))]
	[HarmonyPatch(/*Could not decode attribute arguments.*/)]
	internal class GeoTriggerPatches
	{
		private static bool Prefix()
		{
			return triggersEnabled;
		}
	}

	[HarmonyPatch(typeof(GorillaNetworkDisconnectTrigger))]
	[HarmonyPatch(/*Could not decode attribute arguments.*/)]
	internal class DisconnectTriggerPatches
	{
		private static bool Prefix()
		{
			return triggersEnabled;
		}
	}

	[HarmonyPatch(typeof(GorillaNetworkJoinTrigger))]
	[HarmonyPatch(/*Could not decode attribute arguments.*/)]
	internal class JoinTriggerPatches
	{
		private static bool Prefix()
		{
			return triggersEnabled;
		}
	}

	[HarmonyPatch(typeof(GorillaQuitBox))]
	[HarmonyPatch(/*Could not decode attribute arguments.*/)]
	internal class QuitTriggerPatches
	{
		private static bool Prefix()
		{
			if (!triggersEnabled)
			{
				Logging.Debug("Player fell out of map, disabling noclip");
				((Behaviour)NoClip.Instance).enabled = false;
			}
			return triggersEnabled;
		}
	}

	[HarmonyPatch(typeof(GorillaSetZoneTrigger))]
	[HarmonyPatch(/*Could not decode attribute arguments.*/)]
	internal class ZoneTriggerPatches
	{
		private static bool Prefix()
		{
			return triggersEnabled;
		}
	}

	[HarmonyPatch(typeof(GorillaKeyboardButton))]
	[HarmonyPatch(/*Could not decode attribute arguments.*/)]
	internal class KeyboardButtonPatches
	{
		private static bool Prefix()
		{
			return triggersEnabled;
		}
	}

	public static bool triggersEnabled = true;
}
