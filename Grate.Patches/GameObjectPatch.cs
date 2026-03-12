using HarmonyLib;
using UnityEngine;

namespace Grate.Patches;

[HarmonyPatch(typeof(GameObject))]
[HarmonyPatch(/*Could not decode attribute arguments.*/)]
internal class GameObjectPatch
{
	private static void Postfix(GameObject __result)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		__result.GetComponent<Renderer>().material.shader = Shader.Find("GorillaTag/UberShader");
		__result.GetComponent<Renderer>().material.color = Color.white;
	}
}
