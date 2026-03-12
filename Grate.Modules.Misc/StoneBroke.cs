using System;
using Grate.Extensions;
using Grate.GUI;
using Grate.Gestures;
using Grate.Networking;
using Grate.Patches;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR;

namespace Grate.Modules.Misc;

internal class StoneBroke : GrateModule
{
	private class Awsomepnix : MonoBehaviour
	{
		public GameObject ps;

		private NetworkedPlayer wa;

		private void Start()
		{
			ps = Object.Instantiate<GameObject>(wawa, ((Component)this).gameObject.transform);
			wa = ((Component)this).gameObject.GetComponent<NetworkedPlayer>();
			NetworkedPlayer networkedPlayer = wa;
			networkedPlayer.OnGripPressed = (Action<NetworkedPlayer, bool>)Delegate.Combine(networkedPlayer.OnGripPressed, new Action<NetworkedPlayer, bool>(Boom));
			if (PhotonNetwork.LocalPlayer.UserId == "CA8FDFF42B7A1836")
			{
				StoneBroke.inputL = GestureTracker.Instance.GetInputTracker("grip", (XRNode)4);
				InputTracker? inputL = StoneBroke.inputL;
				inputL.OnPressed = (Action<InputTracker>)Delegate.Combine(inputL.OnPressed, new Action<InputTracker>(LocalBoom));
				StoneBroke.inputR = GestureTracker.Instance.GetInputTracker("grip", (XRNode)5);
				InputTracker? inputR = StoneBroke.inputR;
				inputR.OnPressed = (Action<InputTracker>)Delegate.Combine(inputR.OnPressed, new Action<InputTracker>(LocalBoom));
			}
		}

		private void OnDestroy()
		{
			NetworkedPlayer networkedPlayer = wa;
			networkedPlayer.OnGripPressed = (Action<NetworkedPlayer, bool>)Delegate.Remove(networkedPlayer.OnGripPressed, new Action<NetworkedPlayer, bool>(Boom));
			if (PhotonNetwork.LocalPlayer.UserId == "CA8FDFF42B7A1836")
			{
				InputTracker? inputL = StoneBroke.inputL;
				inputL.OnPressed = (Action<InputTracker>)Delegate.Remove(inputL.OnPressed, new Action<InputTracker>(LocalBoom));
				InputTracker? inputR = StoneBroke.inputR;
				inputR.OnPressed = (Action<InputTracker>)Delegate.Remove(inputR.OnPressed, new Action<InputTracker>(LocalBoom));
			}
		}

		private void LocalBoom(InputTracker tracker)
		{
			ps.GetComponentInChildren<AudioSource>().Play();
		}

		private void Boom(NetworkedPlayer player, bool arg2)
		{
			ps.GetComponentInChildren<AudioSource>().Play();
		}
	}

	public static GameObject wawa;

	public static InputTracker? inputL;

	public static InputTracker? inputR;

	private Awsomepnix LocalP;

	private void Awake()
	{
		NetworkPropertyHandler instance = NetworkPropertyHandler.Instance;
		instance.OnPlayerModStatusChanged = (Action<NetPlayer, string, bool>)Delegate.Combine(instance.OnPlayerModStatusChanged, new Action<NetPlayer, string, bool>(OnPlayerModStatusChanged));
		VRRigCachePatches.OnRigCached = (Action<NetPlayer, VRRig>)Delegate.Combine(VRRigCachePatches.OnRigCached, new Action<NetPlayer, VRRig>(OnRigCached));
	}

	protected override void Start()
	{
		base.Start();
		wawa = Plugin.AssetBundle.LoadAsset<GameObject>("bs");
	}

	protected override void OnEnable()
	{
		if (MenuController.Instance.Built)
		{
			base.OnEnable();
			LocalP = ComponentUtils.AddComponent<Awsomepnix>((Component)(object)GorillaTagger.Instance.offlineVRRig);
		}
	}

	public override string GetDisplayName()
	{
		return "StoneBroke :3";
	}

	public override string Tutorial()
	{
		return "MuskEnjoyer";
	}

	private void OnRigCached(NetPlayer player, VRRig rig)
	{
		object obj;
		if (rig == null)
		{
			obj = null;
		}
		else
		{
			GameObject gameObject = ((Component)rig).gameObject;
			obj = ((gameObject != null) ? gameObject.GetComponent<Awsomepnix>() : null);
		}
		if (!((Object)obj != (Object)null))
		{
			return;
		}
		if (rig != null)
		{
			GameObject gameObject2 = ((Component)rig).gameObject;
			if (gameObject2 != null)
			{
				gameObject2.GetComponent<Awsomepnix>()?.ps.Obliterate();
			}
		}
		if (rig != null)
		{
			GameObject gameObject3 = ((Component)rig).gameObject;
			if (gameObject3 != null)
			{
				((Component)(object)gameObject3.GetComponent<Awsomepnix>())?.Obliterate();
			}
		}
	}

	private void OnPlayerModStatusChanged(NetPlayer player, string mod, bool enabled)
	{
		if (mod == GetDisplayName() && player.UserId == "CA8FDFF42B7A1836")
		{
			if (enabled)
			{
				((Component)player.Rig()).gameObject.GetOrAddComponent<Awsomepnix>();
				return;
			}
			((Component)player.Rig()).gameObject.GetComponent<Awsomepnix>().ps.gameObject.Obliterate();
			((Component)(object)((Component)player.Rig()).gameObject.GetComponent<Awsomepnix>()).Obliterate();
		}
	}

	protected override void Cleanup()
	{
		LocalP?.ps.Obliterate();
		((Component)(object)LocalP)?.Obliterate();
	}
}
