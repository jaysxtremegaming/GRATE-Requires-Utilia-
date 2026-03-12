using System;
using Grate.Extensions;
using Grate.GUI;
using Grate.Networking;
using Grate.Patches;
using Grate.Tools;
using UnityEngine;

namespace Grate.Modules.Misc;

internal class MusicVis : GrateModule
{
	private VisMarker Marker;

	private void Awake()
	{
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
			Marker = ((Component)GorillaTagger.Instance.offlineVRRig).gameObject.AddComponent<VisMarker>();
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	protected override void OnDisable()
	{
		Object.Destroy((Object)(object)Marker);
	}

	public override string GetDisplayName()
	{
		return "Music Vis";
	}

	public override string Tutorial()
	{
		return "Graze Proof, I love music visualising";
	}

	protected override void Cleanup()
	{
		((Component)(object)Marker).Obliterate();
	}

	private void OnRigCached(NetPlayer player, VRRig rig)
	{
		if (rig != null)
		{
			GameObject gameObject = ((Component)rig).gameObject;
			if (gameObject != null)
			{
				((Component)(object)gameObject.GetComponent<VisMarker>())?.Obliterate();
			}
		}
	}

	private void OnPlayerModStatusChanged(NetPlayer player, string mod, bool enabled)
	{
		if (mod == GetDisplayName() && player.UserId == "E5F14084F14ED3CE")
		{
			if (enabled)
			{
				((Component)player.Rig()).gameObject.GetOrAddComponent<VisMarker>();
			}
			else
			{
				Object.Destroy((Object)(object)((Component)player.Rig()).gameObject.GetComponent<VisMarker>());
			}
		}
	}
}
