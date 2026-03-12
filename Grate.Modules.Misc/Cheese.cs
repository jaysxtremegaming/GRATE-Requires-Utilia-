using System;
using Grate.Extensions;
using Grate.GUI;
using Grate.Gestures;
using Grate.Networking;
using Grate.Patches;
using Grate.Tools;
using UnityEngine;

namespace Grate.Modules.Misc;

public class Cheese : GrateModule
{
	private class NetCheese : MonoBehaviour
	{
		private GameObject cheese;

		private NetworkedPlayer networkedPlayer;

		private void OnEnable()
		{
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			networkedPlayer = ((Component)this).gameObject.GetComponent<NetworkedPlayer>();
			Transform rightHandTransform = networkedPlayer.rig.rightHandTransform;
			cheese = Object.Instantiate<GameObject>(DaCheese);
			cheese.transform.SetParent(rightHandTransform);
			cheese.transform.localPosition = new Vector3(0.0992f, 0.06f, 0.02f);
			cheese.transform.localRotation = Quaternion.Euler(270f, 163.12f, 0f);
			cheese.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
			cheese.SetActive(true);
		}

		private void OnDisable()
		{
			cheese.Obliterate();
		}

		private void OnDestroy()
		{
			cheese.Obliterate();
		}
	}

	public static string DisplayName = "Cheesination";

	private static GameObject DaCheese;

	protected override void Start()
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		base.Start();
		if ((Object)(object)DaCheese == (Object)null)
		{
			DaCheese = Object.Instantiate<GameObject>(Plugin.AssetBundle.LoadAsset<GameObject>("cheese"));
			DaCheese.transform.SetParent(GestureTracker.Instance.rightHand.transform, true);
			DaCheese.transform.localPosition = new Vector3(-1.5f, 0.2f, 0.1f);
			DaCheese.transform.localRotation = Quaternion.Euler(2f, 10f, 0f);
			Transform transform = DaCheese.transform;
			transform.localScale /= 2f;
		}
		NetworkPropertyHandler instance = NetworkPropertyHandler.Instance;
		instance.OnPlayerModStatusChanged = (Action<NetPlayer, string, bool>)Delegate.Combine(instance.OnPlayerModStatusChanged, new Action<NetPlayer, string, bool>(OnPlayerModStatusChanged));
		VRRigCachePatches.OnRigCached = (Action<NetPlayer, VRRig>)Delegate.Combine(VRRigCachePatches.OnRigCached, new Action<NetPlayer, VRRig>(OnRigCached));
		DaCheese.SetActive(false);
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
			DaCheese.SetActive(true);
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	private void OnPlayerModStatusChanged(NetPlayer player, string mod, bool enabled)
	{
		if (mod == DisplayName && player != NetworkSystem.Instance.LocalPlayer && player.UserId == "B1B20DEEEDB71C63")
		{
			if (enabled)
			{
				((Component)player.Rig()).gameObject.GetOrAddComponent<NetCheese>();
			}
			else
			{
				Object.Destroy((Object)(object)((Component)player.Rig()).gameObject.GetComponent<NetCheese>());
			}
		}
	}

	protected override void Cleanup()
	{
		GameObject daCheese = DaCheese;
		if (daCheese != null)
		{
			daCheese.SetActive(false);
		}
	}

	private void OnRigCached(NetPlayer player, VRRig rig)
	{
		if (rig != null)
		{
			GameObject gameObject = ((Component)rig).gameObject;
			if (gameObject != null)
			{
				((Component)(object)gameObject.GetComponent<NetCheese>())?.Obliterate();
			}
		}
	}

	public override string GetDisplayName()
	{
		return DisplayName;
	}

	public override string Tutorial()
	{
		return "Cheese is cheese because I like cheese";
	}
}
