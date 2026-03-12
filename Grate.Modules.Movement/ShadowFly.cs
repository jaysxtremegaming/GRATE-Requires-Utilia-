using System;
using Grate.Extensions;
using Grate.GUI;
using Grate.Networking;
using Grate.Patches;
using UnityEngine;

namespace Grate.Modules.Movement;

public class ShadowFly : GrateModule
{
	private class NetShadWing : MonoBehaviour
	{
		private GameObject netWings;

		private NetworkedPlayer networkedPlayer;

		private void OnEnable()
		{
			networkedPlayer = ((Component)this).gameObject.GetComponent<NetworkedPlayer>();
			netWings = Object.Instantiate<GameObject>(localWings, ((Component)networkedPlayer.rig).transform);
			netWings.SetActive(true);
		}

		private void OnDisable()
		{
			netWings.Obliterate();
		}

		private void OnDestroy()
		{
			netWings.Obliterate();
		}
	}

	private static GameObject? localWings;

	public static string DisplayName = "Shadow Fly";

	protected override void Start()
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		base.Start();
		if ((Object)(object)localWings == (Object)null)
		{
			AssetBundle? assetBundle = Plugin.AssetBundle;
			localWings = Object.Instantiate<GameObject>((assetBundle != null) ? assetBundle.LoadAsset<GameObject>("ShadowWings") : null, ((Component)VRRig.LocalRig).transform);
			localWings.transform.localScale = Vector3.one;
		}
		localWings.SetActive(false);
		NetworkPropertyHandler instance = NetworkPropertyHandler.Instance;
		instance.OnPlayerModStatusChanged = (Action<NetPlayer, string, bool>)Delegate.Combine(instance.OnPlayerModStatusChanged, new Action<NetPlayer, string, bool>(OnPlayerModStatusChanged));
		VRRigCachePatches.OnRigCached = (Action<NetPlayer, VRRig>)Delegate.Combine(VRRigCachePatches.OnRigCached, new Action<NetPlayer, VRRig>(OnRigCached));
	}

	protected override void OnEnable()
	{
		if (MenuController.Instance.Built)
		{
			base.OnEnable();
			localWings.SetActive(true);
		}
	}

	private void OnPlayerModStatusChanged(NetPlayer player, string mod, bool enabled)
	{
		if (mod == GetDisplayName() && player != NetworkSystem.Instance.LocalPlayer && player.UserId == "AE10C04744CCF6E7")
		{
			if (enabled)
			{
				((Component)player.Rig()).gameObject.GetOrAddComponent<NetShadWing>();
			}
			else
			{
				Object.Destroy((Object)(object)((Component)player.Rig()).gameObject.GetComponent<NetShadWing>());
			}
		}
	}

	protected override void Cleanup()
	{
		if (Object.op_Implicit((Object)(object)localWings))
		{
			localWings.SetActive(false);
		}
	}

	private void OnRigCached(NetPlayer player, VRRig rig)
	{
		if (rig != null)
		{
			GameObject gameObject = ((Component)rig).gameObject;
			if (gameObject != null)
			{
				((Component)(object)gameObject.GetComponent<NetShadWing>())?.Obliterate();
			}
		}
	}

	public override string Tutorial()
	{
		return "- Cool wings for a tier 3 supporter";
	}

	public override string GetDisplayName()
	{
		return DisplayName;
	}
}
