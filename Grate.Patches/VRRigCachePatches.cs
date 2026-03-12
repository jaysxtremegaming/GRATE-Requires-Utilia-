using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;

namespace Grate.Patches;

[HarmonyPatch]
public class VRRigCachePatches
{
	public static Action<NetPlayer, VRRig> OnRigCached;

	private static IEnumerable<MethodBase> TargetMethods()
	{
		return new MethodBase[1] { AccessTools.Method("VRRigCache:RemoveRigFromGorillaParent", (Type[])null, (Type[])null) };
	}

	private static void Postfix(NetPlayer player, VRRig vrrig)
	{
		OnRigCached?.Invoke(player, vrrig);
	}
}
