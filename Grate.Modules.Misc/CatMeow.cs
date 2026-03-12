using System;
using System.Collections.Generic;
using Grate.Extensions;
using Grate.GUI;
using Grate.Gestures;
using Grate.Networking;
using Grate.Patches;
using Photon.Pun;
using UnityEngine;
using UnityEngine.XR;

namespace Grate.Modules.Misc;

internal class CatMeow : GrateModule
{
	private class TheMeower : MonoBehaviour
	{
		private AudioSource meowAudioNet;

		private GameObject meowboxNet;

		private ParticleSystem meowParticlesNet;

		private NetworkedPlayer netPlayer;

		private VRRig rigNet;

		private void Start()
		{
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			if (PhotonNetwork.LocalPlayer.UserId == "FBE3EE50747CB892")
			{
				rigNet = ((Component)this).GetComponent<VRRig>();
				netPlayer = ((Component)rigNet).GetComponent<NetworkedPlayer>();
				meowboxNet = Object.Instantiate<GameObject>(meowerPrefab, ((Component)rigNet).gameObject.transform);
				meowboxNet.transform.localPosition = Vector3.zero;
				meowParticlesNet = meowboxNet.GetComponent<ParticleSystem>();
				meowAudioNet = meowboxNet.GetComponent<AudioSource>();
				NetworkedPlayer networkedPlayer = netPlayer;
				networkedPlayer.OnGripPressed = (Action<NetworkedPlayer, bool>)Delegate.Combine(networkedPlayer.OnGripPressed, new Action<NetworkedPlayer, bool>(DoMeowNetworked));
			}
			else
			{
				Object.Destroy((Object)(object)this);
			}
		}

		private void OnDestroy()
		{
			NetworkedPlayer networkedPlayer = netPlayer;
			networkedPlayer.OnGripPressed = (Action<NetworkedPlayer, bool>)Delegate.Remove(networkedPlayer.OnGripPressed, new Action<NetworkedPlayer, bool>(DoMeowNetworked));
		}

		private void DoMeowNetworked(NetworkedPlayer player, bool isLeft)
		{
			DoMeow(meowParticlesNet, meowAudioNet);
		}
	}

	private static readonly List<AudioClip> meowSounds = new List<AudioClip>();

	private static GameObject meowerPrefab;

	private static readonly Random rnd = new Random();

	public static string DisplayName = "Meow";

	private readonly InputTracker? inputL = GestureTracker.Instance.GetInputTracker("grip", (XRNode)4);

	private readonly InputTracker? inputR = GestureTracker.Instance.GetInputTracker("grip", (XRNode)5);

	private AudioSource meowAudio;

	private GameObject meowbox;

	private ParticleSystem meowParticles;

	private VRRig rig;

	private void Awake()
	{
		NetworkPropertyHandler instance = NetworkPropertyHandler.Instance;
		instance.OnPlayerModStatusChanged = (Action<NetPlayer, string, bool>)Delegate.Combine(instance.OnPlayerModStatusChanged, new Action<NetPlayer, string, bool>(OnPlayerModStatusChanged));
		VRRigCachePatches.OnRigCached = (Action<NetPlayer, VRRig>)Delegate.Combine(VRRigCachePatches.OnRigCached, new Action<NetPlayer, VRRig>(OnRigCached));
	}

	protected override void Start()
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		base.Start();
		try
		{
			rig = GorillaTagger.Instance.offlineVRRig;
			meowerPrefab = Plugin.AssetBundle.LoadAsset<GameObject>("ParticleEmitter");
			meowbox = Object.Instantiate<GameObject>(meowerPrefab, ((Component)rig).gameObject.transform);
			meowbox.transform.localPosition = Vector3.zero;
			meowParticles = meowbox.GetComponent<ParticleSystem>();
			meowAudio = meowbox.GetComponent<AudioSource>();
			meowSounds.Add(Plugin.AssetBundle.LoadAsset<AudioClip>("meow1"));
			meowSounds.Add(Plugin.AssetBundle.LoadAsset<AudioClip>("meow2"));
			meowSounds.Add(Plugin.AssetBundle.LoadAsset<AudioClip>("meow3"));
			meowSounds.Add(Plugin.AssetBundle.LoadAsset<AudioClip>("meow4"));
		}
		catch
		{
		}
	}

	protected override void OnEnable()
	{
		if (MenuController.Instance.Built && !(PhotonNetwork.LocalPlayer.UserId != "FBE3EE50747CB892"))
		{
			base.OnEnable();
			GripOn();
		}
	}

	protected override void OnDisable()
	{
		GripOff();
	}

	public override string GetDisplayName()
	{
		return DisplayName;
	}

	public override string Tutorial()
	{
		return "Mrrrrpp....";
	}

	protected override void Cleanup()
	{
		GripOff();
	}

	private void OnLocalGrip(InputTracker _)
	{
		DoMeow(meowParticles, meowAudio);
	}

	private void GripOn()
	{
		InputTracker? inputTracker = inputL;
		inputTracker.OnPressed = (Action<InputTracker>)Delegate.Combine(inputTracker.OnPressed, new Action<InputTracker>(OnLocalGrip));
		InputTracker? inputTracker2 = inputR;
		inputTracker2.OnPressed = (Action<InputTracker>)Delegate.Combine(inputTracker2.OnPressed, new Action<InputTracker>(OnLocalGrip));
	}

	private void GripOff()
	{
		InputTracker? inputTracker = inputL;
		inputTracker.OnPressed = (Action<InputTracker>)Delegate.Remove(inputTracker.OnPressed, new Action<InputTracker>(OnLocalGrip));
		InputTracker? inputTracker2 = inputR;
		inputTracker2.OnPressed = (Action<InputTracker>)Delegate.Remove(inputTracker2.OnPressed, new Action<InputTracker>(OnLocalGrip));
	}

	private void OnRigCached(NetPlayer player, VRRig rig)
	{
		if (rig != null)
		{
			GameObject gameObject = ((Component)rig).gameObject;
			if (gameObject != null)
			{
				((Component)(object)gameObject.GetComponent<TheMeower>())?.Obliterate();
			}
		}
	}

	private void OnPlayerModStatusChanged(NetPlayer player, string mod, bool enabled)
	{
		if (mod == GetDisplayName() && player.UserId == "FBE3EE50747CB892")
		{
			if (enabled)
			{
				((Component)player.Rig()).gameObject.GetOrAddComponent<TheMeower>();
			}
			else
			{
				Object.Destroy((Object)(object)((Component)player.Rig()).gameObject.GetComponent<TheMeower>());
			}
		}
	}

	private static void DoMeow(ParticleSystem meowParticles, AudioSource meowAudioSource)
	{
		meowAudioSource.PlayOneShot(meowSounds[rnd.Next(meowSounds.Count)]);
		meowParticles.Play();
		meowParticles.Emit(1);
	}
}
