using System;
using System.Collections.Generic;
using System.Linq;
using GorillaLocomotion;
using Grate.Modules;
using HarmonyLib;
using Photon.Pun;
using UnityEngine;

namespace Grate.Extensions;

public static class PlayerExtensions
{
	private static readonly HashSet<string> DeveloperIds = new HashSet<string> { "42D7D32651E93866", "9ABD0C174289F58E", "B1B20DEEEDB71C63", "A48744B93D9A3596" };

	public static void AddForce(this GTPlayer self, Vector3 v)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		Rigidbody component = ((Component)self).GetComponent<Rigidbody>();
		component.velocity += v;
	}

	public static void SetVelocity(this GTPlayer self, Vector3 v)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		((Component)self).GetComponent<Rigidbody>().velocity = v;
	}

	public static PhotonView PhotonView(this VRRig rig)
	{
		return Traverse.Create((object)rig).Field("photonView").GetValue<PhotonView>();
	}

	public static bool HasProperty(this VRRig rig, string key)
	{
		return (rig?.OwningNetPlayer?.HasProperty(key)).GetValueOrDefault();
	}

	public static bool ModuleEnabled(this VRRig rig, string mod)
	{
		return (rig?.OwningNetPlayer?.ModuleEnabled(mod)).GetValueOrDefault();
	}

	public static T GetProperty<T>(this NetPlayer? player, string key)
	{
		return (T)((player != null) ? player.GetPlayerRef().CustomProperties[(object)key] : null);
	}

	public static bool HasProperty(this NetPlayer player, string key)
	{
		if (player == null)
		{
			return false;
		}
		return ((Dictionary<object, object>)(object)player.GetPlayerRef().CustomProperties).ContainsKey((object)key);
	}

	public static bool ModuleEnabled(this NetPlayer player, string mod)
	{
		if (!player.HasProperty(GrateModule.enabledModulesKey))
		{
			return false;
		}
		bool value;
		return player.GetProperty<Dictionary<string, bool>>(GrateModule.enabledModulesKey).TryGetValue(mod, out value) && value;
	}

	public static VRRig? Rig(this NetPlayer? player)
	{
		NetPlayer player2 = player;
		return ((IEnumerable<VRRig>)((GorillaParent)GorillaParent.instance).vrrigs).FirstOrDefault((Func<VRRig, bool>)((VRRig rig) => rig.OwningNetPlayer == player2));
	}

	public static bool IsDev(this NetPlayer player)
	{
		return DeveloperIds.Contains(player.UserId);
	}

	public static bool IsAdmin(this NetPlayer player)
	{
		return player.IsDev();
	}

	public static bool IsSupporter(this NetPlayer player)
	{
		return player.IsDev();
	}
}
