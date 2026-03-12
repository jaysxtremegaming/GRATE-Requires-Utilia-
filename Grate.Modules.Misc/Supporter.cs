using System;
using Grate.Extensions;
using Grate.GUI;
using Grate.Gestures;
using Grate.Networking;
using Grate.Patches;
using Grate.Tools;
using UnityEngine;

namespace Grate.Modules.Misc;

public class Supporter : GrateModule
{
	private class NetPhone : MonoBehaviour
	{
		private NetworkedPlayer? networkedPlayer;

		private GameObject? phone;

		private void OnEnable()
		{
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			networkedPlayer = ((Component)this).gameObject.GetComponent<NetworkedPlayer>();
			Transform rightHandTransform = networkedPlayer.rig.rightHandTransform;
			phone = Object.Instantiate<GameObject>(_phone, rightHandTransform, false);
			if (!((Object)(object)phone == (Object)null))
			{
				phone.transform.localPosition = new Vector3(0.0992f, 0.06f, 0.02f);
				phone.transform.localRotation = Quaternion.Euler(270f, 163.12f, 0f);
				Vector3 localScale = phone.transform.localScale / 20f;
				localScale.y = 54f;
				phone.transform.localScale = localScale;
				phone.SetActive(true);
			}
		}

		private void OnDisable()
		{
			phone?.Obliterate();
		}

		private void OnDestroy()
		{
			phone?.Obliterate();
		}
	}

	private static readonly string DisplayName = "Trusted Phone";

	private static GameObject? _phone;

	protected override void Start()
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		base.Start();
		if ((Object)(object)_phone == (Object)null)
		{
			_phone = Object.Instantiate<GameObject>(Plugin.AssetBundle.LoadAsset<GameObject>("PHONE"));
			_phone.transform.SetParent(GestureTracker.Instance.rightHand.transform, true);
			_phone.transform.localPosition = new Vector3(-1.5f, 0.2f, 0.1f);
			_phone.transform.localRotation = Quaternion.Euler(2f, 10f, 0f);
			Transform transform = _phone.transform;
			transform.localScale /= 2f;
		}
		NetworkPropertyHandler instance = NetworkPropertyHandler.Instance;
		instance.OnPlayerModStatusChanged = (Action<NetPlayer, string, bool>)Delegate.Combine(instance.OnPlayerModStatusChanged, new Action<NetPlayer, string, bool>(OnPlayerModStatusChanged));
		VRRigCachePatches.OnRigCached = (Action<NetPlayer, VRRig>)Delegate.Combine(VRRigCachePatches.OnRigCached, new Action<NetPlayer, VRRig>(OnRigCached));
		GameObject? phone = _phone;
		if (phone != null)
		{
			phone.SetActive(false);
		}
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
			GameObject? phone = _phone;
			if (phone != null)
			{
				phone.SetActive(true);
			}
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	private void OnPlayerModStatusChanged(NetPlayer player, string mod, bool modEnabled)
	{
		if (mod != DisplayName || !player.IsSupporter())
		{
			return;
		}
		if (modEnabled)
		{
			VRRig? obj = player.Rig();
			if (obj != null)
			{
				((Component)obj).gameObject.GetOrAddComponent<NetPhone>();
			}
		}
		else
		{
			VRRig? obj2 = player.Rig();
			Object.Destroy((Object)(object)((obj2 != null) ? ((Component)obj2).gameObject.GetComponent<NetPhone>() : null));
		}
	}

	protected override void Cleanup()
	{
		GameObject? phone = _phone;
		if (phone != null)
		{
			phone.SetActive(false);
		}
	}

	private static void OnRigCached(NetPlayer player, VRRig rig)
	{
		if (rig != null)
		{
			GameObject gameObject = ((Component)rig).gameObject;
			if (gameObject != null)
			{
				((Component)(object)gameObject.GetComponent<NetPhone>())?.Obliterate();
			}
		}
	}

	public override string GetDisplayName()
	{
		return DisplayName;
	}

	public override string Tutorial()
	{
		return "given out to people the grate developers and the Supporters";
	}
}
