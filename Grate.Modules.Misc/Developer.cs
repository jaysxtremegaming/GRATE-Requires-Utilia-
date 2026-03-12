using System;
using Grate.Extensions;
using Grate.GUI;
using Grate.Gestures;
using Grate.Networking;
using Grate.Patches;
using Grate.Tools;
using UnityEngine;

namespace Grate.Modules.Misc;

public class Developer : GrateModule
{
	private class NetDevPhone : MonoBehaviour
	{
		private NetworkedPlayer networkedPlayer;

		private GameObject phone;

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
			phone = Object.Instantiate<GameObject>(Phone);
			phone.transform.SetParent(rightHandTransform);
			phone.transform.localPosition = new Vector3(0.0992f, 0.06f, 0.02f);
			phone.transform.localRotation = Quaternion.Euler(270f, 163.12f, 0f);
			Vector3 localScale = phone.transform.localScale / 20f;
			localScale.y = 54f;
			phone.transform.localScale = localScale;
			phone.SetActive(true);
		}

		private void OnDisable()
		{
			phone.Obliterate();
		}

		private void OnDestroy()
		{
			phone.Obliterate();
		}
	}

	public static string DisplayName = "Dev Phone";

	private static GameObject Phone;

	protected override void Start()
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		base.Start();
		if ((Object)(object)Phone == (Object)null)
		{
			Phone = Object.Instantiate<GameObject>(Plugin.AssetBundle.LoadAsset<GameObject>("DEVPHONE"));
			Phone.transform.SetParent(GestureTracker.Instance.rightHand.transform, true);
			Phone.transform.localPosition = new Vector3(-1.5f, 0.2f, 0.1f);
			Phone.transform.localRotation = Quaternion.Euler(2f, 10f, 0f);
			Transform transform = Phone.transform;
			transform.localScale /= 2f;
		}
		NetworkPropertyHandler instance = NetworkPropertyHandler.Instance;
		instance.OnPlayerModStatusChanged = (Action<NetPlayer, string, bool>)Delegate.Combine(instance.OnPlayerModStatusChanged, new Action<NetPlayer, string, bool>(OnPlayerModStatusChanged));
		VRRigCachePatches.OnRigCached = (Action<NetPlayer, VRRig>)Delegate.Combine(VRRigCachePatches.OnRigCached, new Action<NetPlayer, VRRig>(OnRigCached));
		Phone.SetActive(false);
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
			Phone.SetActive(true);
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	private void OnPlayerModStatusChanged(NetPlayer player, string mod, bool enabled)
	{
		if (mod == DisplayName && player.IsDev())
		{
			if (enabled)
			{
				((Component)player.Rig()).gameObject.GetOrAddComponent<NetDevPhone>();
			}
			else
			{
				Object.Destroy((Object)(object)((Component)player.Rig()).gameObject.GetComponent<NetDevPhone>());
			}
		}
	}

	protected override void Cleanup()
	{
		GameObject phone = Phone;
		if (phone != null)
		{
			phone.SetActive(false);
		}
	}

	private void OnRigCached(NetPlayer player, VRRig rig)
	{
		if (rig != null)
		{
			GameObject gameObject = ((Component)rig).gameObject;
			if (gameObject != null)
			{
				((Component)(object)gameObject.GetComponent<NetDevPhone>())?.Obliterate();
			}
		}
	}

	public override string GetDisplayName()
	{
		return DisplayName;
	}

	public override string Tutorial()
	{
		return "Given to the devs";
	}
}
