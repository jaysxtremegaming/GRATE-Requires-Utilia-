using System;
using Grate.Extensions;
using Grate.GUI;
using Grate.Networking;
using Grate.Patches;
using Grate.Tools;
using UnityEngine;

namespace Grate.Modules.Misc;

public class Halo : GrateModule
{
	public static readonly string DisplayName = "Halo";

	public static GameObject haloPrefab;

	public static GameObject lightBeamPrefab;

	private HaloMarker myMarker;

	private void Awake()
	{
		if (!Object.op_Implicit((Object)(object)haloPrefab))
		{
			haloPrefab = Plugin.AssetBundle.LoadAsset<GameObject>("Halo");
			lightBeamPrefab = Plugin.AssetBundle.LoadAsset<GameObject>("Light Beam");
		}
		NetworkPropertyHandler instance = NetworkPropertyHandler.Instance;
		instance.OnPlayerModStatusChanged = (Action<NetPlayer, string, bool>)Delegate.Combine(instance.OnPlayerModStatusChanged, new Action<NetPlayer, string, bool>(OnPlayerModStatusChanged));
		VRRigCachePatches.OnRigCached = (Action<NetPlayer, VRRig>)Delegate.Combine(VRRigCachePatches.OnRigCached, new Action<NetPlayer, VRRig>(OnRigCached));
	}

	protected override void OnEnable()
	{
		if (!MenuController.Instance.Built)
		{
			return;
		}
		base.OnEnable();
		try
		{
			myMarker = ((Component)GorillaTagger.Instance.offlineVRRig).gameObject.AddComponent<HaloMarker>();
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	private void OnRigCached(NetPlayer player, VRRig rig)
	{
		if (rig != null)
		{
			GameObject gameObject = ((Component)rig).gameObject;
			if (gameObject != null)
			{
				((Component)(object)gameObject.GetComponent<HaloMarker>())?.Obliterate();
			}
		}
	}

	private void OnPlayerModStatusChanged(NetPlayer player, string mod, bool enabled)
	{
		if (mod == DisplayName && player.UserId == "JD3moEFc6tOGYSAp4MjKsIwVycfrAUR5nLkkDNSvyvE=".DecryptString())
		{
			if (enabled)
			{
				((Component)player.Rig()).gameObject.GetOrAddComponent<HaloMarker>();
			}
			else
			{
				Object.Destroy((Object)(object)((Component)player.Rig()).gameObject.GetComponent<HaloMarker>());
			}
		}
	}

	protected override void Cleanup()
	{
		Object.Destroy((Object)(object)myMarker);
	}

	public override string GetDisplayName()
	{
		return DisplayName;
	}

	public override string Tutorial()
	{
		return "Proof of Kyle";
	}
}
