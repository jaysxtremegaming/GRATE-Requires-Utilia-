using GorillaLocomotion;
using HarmonyLib;

namespace Grate.Patches;

[HarmonyPatch(typeof(GTPlayer), "AntiTeleportTechnology")]
internal class PlayerAntiAntiTeleport
{
	private static bool Prefix()
	{
		return !Plugin.WaWaGrazeDotCc;
	}
}
