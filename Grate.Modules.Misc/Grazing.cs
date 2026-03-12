using System;
using Grate.Extensions;
using Grate.GUI;
using Grate.Networking;
using Grate.Patches;
using UnityEngine;
using UnityEngine.Video;

namespace Grate.Modules.Misc;

public class Grazing : GrateModule
{
	public class MuteButton : GorillaPressableButton
	{
		private bool muted;

		public override void ButtonActivation()
		{
			((GorillaPressableButton)this).ButtonActivation();
			muted = !muted;
			((Component)((Component)this).transform.parent).GetComponentInChildren<AudioSource>().mute = muted;
		}
	}

	public class GrazeHandler : MonoBehaviour
	{
		private NetworkedPlayer? np;

		private GameObject? tv;

		public VideoPlayer? vp;

		private void Start()
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Expected O, but got Unknown
			np = ((Component)this).GetComponent<NetworkedPlayer>();
			tv = Object.Instantiate<GameObject>(_tv);
			tv.transform.position = np.rig.headConstraint.position + Vector3.up * 0.25f + Vector3.forward * 0.25f;
			tv.transform.rotation = np.rig.syncRotation;
			GameObject? obj = tv;
			vp = ((obj != null) ? ((Component)obj.GetComponentInChildren<VideoPlayer>()).GetComponentInChildren<VideoPlayer>() : null);
			vp.loopPointReached += (EventHandler)delegate
			{
				vp.Play();
			};
		}

		private void Update()
		{
			if (Object.op_Implicit((Object)(object)np))
			{
				NetworkedPlayer? networkedPlayer = np;
				object obj;
				if (networkedPlayer == null)
				{
					obj = null;
				}
				else
				{
					NetPlayer? owner = networkedPlayer.owner;
					obj = ((owner != null) ? owner.UserId : null);
				}
				if ((string?)obj != "42D7D32651E93866")
				{
					((Component)(object)this).Obliterate();
				}
			}
		}

		private void OnDisable()
		{
			OnDestroy();
		}

		private void OnDestroy()
		{
			tv?.Obliterate();
		}
	}

	private static GameObject? _tv;

	private GrazeHandler? localGraze;

	protected override void Start()
	{
		base.Start();
		NetworkPropertyHandler instance = NetworkPropertyHandler.Instance;
		instance.OnPlayerModStatusChanged = (Action<NetPlayer, string, bool>)Delegate.Combine(instance.OnPlayerModStatusChanged, new Action<NetPlayer, string, bool>(OnPlayerModStatusChanged));
		VRRigCachePatches.OnRigCached = (Action<NetPlayer, VRRig>)Delegate.Combine(VRRigCachePatches.OnRigCached, new Action<NetPlayer, VRRig>(OnRigCached));
		AssetBundle? assetBundle = Plugin.AssetBundle;
		_tv = ((assetBundle != null) ? assetBundle.LoadAsset<GameObject>("GrazeTV") : null);
		GameObject? tv = _tv;
		if (tv != null)
		{
			ComponentUtils.AddComponent<MuteButton>((Component)(object)tv.transform.GetChild(1));
		}
	}

	protected override void OnEnable()
	{
		if (MenuController.Instance.Built)
		{
			base.OnEnable();
			localGraze = ComponentUtils.AddComponent<GrazeHandler>((Component)(object)GorillaTagger.Instance.offlineVRRig);
		}
	}

	public override string GetDisplayName()
	{
		return "Gwazywazy";
	}

	public override string Tutorial()
	{
		return "I am me maker of this yes";
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
			obj = ((gameObject != null) ? gameObject.GetComponent<GrazeHandler>() : null);
		}
		if ((Object)obj != (Object)null && rig != null)
		{
			GameObject gameObject2 = ((Component)rig).gameObject;
			if (gameObject2 != null)
			{
				((Component)(object)gameObject2.GetComponent<GrazeHandler>())?.Obliterate();
			}
		}
	}

	private void OnPlayerModStatusChanged(NetPlayer player, string mod, bool modEnabled)
	{
		if (mod != GetDisplayName() || player.UserId != "42D7D32651E93866")
		{
			return;
		}
		if (modEnabled)
		{
			VRRig? obj = player.Rig();
			if (obj != null)
			{
				((Component)obj).gameObject.GetOrAddComponent<GrazeHandler>();
			}
		}
		else
		{
			VRRig? obj2 = player.Rig();
			if (obj2 != null)
			{
				((Component)(object)((Component)obj2).gameObject.GetComponent<GrazeHandler>()).Obliterate();
			}
		}
	}

	protected override void Cleanup()
	{
		((Component)(object)localGraze)?.Obliterate();
	}
}
