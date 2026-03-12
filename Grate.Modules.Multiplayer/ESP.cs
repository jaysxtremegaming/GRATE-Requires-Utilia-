using System;
using System.Collections.Generic;
using Grate.Extensions;
using Grate.Networking;
using Grate.Patches;
using UnityEngine;

namespace Grate.Modules.Multiplayer;

internal class ESP : GrateModule
{
	private readonly Shader esp = Shader.Find("GUI/Text Shader");

	private readonly List<VRRig> Espd = new List<VRRig>();

	private readonly Shader Uber = Shader.Find("GorillaTag/UberShader");

	private void FixedUpdate()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		foreach (VRRig item in Espd)
		{
			((Renderer)item.skeleton.renderer).material.color = Colours(item);
			((Renderer)item.skeleton.renderer).material.shader = esp;
		}
	}

	protected override void OnEnable()
	{
		VRRigCachePatches.OnRigCached = (Action<NetPlayer, VRRig>)Delegate.Combine(VRRigCachePatches.OnRigCached, new Action<NetPlayer, VRRig>(OnRigCached));
		NetworkPropertyHandler instance = NetworkPropertyHandler.Instance;
		instance.OnPlayerJoined = (Action<NetPlayer>)Delegate.Combine(instance.OnPlayerJoined, new Action<NetPlayer>(OnPlayerJoined));
		foreach (VRRig vrrig in ((GorillaParent)GorillaParent.instance).vrrigs)
		{
			if (!vrrig.isOfflineVRRig)
			{
				((Renderer)vrrig.skeleton.renderer).enabled = true;
				((Renderer)vrrig.skeleton.renderer).material.shader = esp;
				Espd.Add(vrrig);
			}
		}
	}

	public override string GetDisplayName()
	{
		return "ESP";
	}

	public override string Tutorial()
	{
		return "Makes You see Players Through Walls!";
	}

	private void OnPlayerJoined(NetPlayer player)
	{
		if (!player.IsLocal)
		{
			((Renderer)player.Rig().skeleton.renderer).enabled = true;
			((Renderer)player.Rig().skeleton.renderer).material.shader = esp;
			Espd.Add(player.Rig());
		}
	}

	private void OnRigCached(NetPlayer player, VRRig rig)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		if (!player.IsLocal)
		{
			((Renderer)rig.skeleton.renderer).enabled = false;
			((Renderer)rig.skeleton.renderer).material.shader = Uber;
			((Renderer)rig.skeleton.renderer).material.color = rig.playerColor;
			if (Espd.Contains(rig))
			{
				Espd.Remove(rig);
			}
		}
	}

	protected override void Cleanup()
	{
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		VRRigCachePatches.OnRigCached = (Action<NetPlayer, VRRig>)Delegate.Remove(VRRigCachePatches.OnRigCached, new Action<NetPlayer, VRRig>(OnRigCached));
		NetworkPropertyHandler instance = NetworkPropertyHandler.Instance;
		instance.OnPlayerJoined = (Action<NetPlayer>)Delegate.Remove(instance.OnPlayerJoined, new Action<NetPlayer>(OnPlayerJoined));
		foreach (VRRig vrrig in ((GorillaParent)GorillaParent.instance).vrrigs)
		{
			((Renderer)vrrig.skeleton.renderer).enabled = false;
			((Renderer)vrrig.skeleton.renderer).material.shader = Uber;
			((Renderer)vrrig.skeleton.renderer).material.color = vrrig.playerColor;
			if (Espd.Contains(vrrig))
			{
				Espd.Remove(vrrig);
			}
		}
	}

	private Color Colours(VRRig rig)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		switch (rig.setMatIndex)
		{
		default:
			return rig.playerColor;
		case 1:
			return Color.red;
		case 2:
		case 11:
			return new Color(1f, 0.3288f, 0f, 1f);
		case 3:
		case 7:
			return Color.blue;
		case 12:
			return Color.green;
		}
	}
}
