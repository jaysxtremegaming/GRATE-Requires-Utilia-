using System;
using Grate.Extensions;
using Grate.GUI;
using Grate.Gestures;
using Grate.Networking;
using Grate.Patches;
using Grate.Tools;
using UnityEngine;

namespace Grate.Modules.Misc;

public class Baggy : GrateModule
{
	private class NetBag : MonoBehaviour
	{
		private GameObject Bag;

		private NetworkedPlayer networkedPlayer;

		private void OnEnable()
		{
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			networkedPlayer = ((Component)this).gameObject.GetComponent<NetworkedPlayer>();
			Transform rightHandTransform = networkedPlayer.rig.rightHandTransform;
			Bag = Object.Instantiate<GameObject>(Baggy.Bag);
			Bag.transform.SetParent(rightHandTransform);
			Bag.transform.localPosition = new Vector3(0.04f, 0.05f, -0.02f);
			Bag.transform.localRotation = Quaternion.Euler(270f, 163.12f, 0f);
			Bag.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
			NetworkedPlayer obj = networkedPlayer;
			obj.OnGripPressed = (Action<NetworkedPlayer, bool>)Delegate.Combine(obj.OnGripPressed, new Action<NetworkedPlayer, bool>(OnGripPressed));
			NetworkedPlayer obj2 = networkedPlayer;
			obj2.OnGripReleased = (Action<NetworkedPlayer, bool>)Delegate.Combine(obj2.OnGripReleased, new Action<NetworkedPlayer, bool>(OnGripReleased));
		}

		private void OnDisable()
		{
			Bag.Obliterate();
			NetworkedPlayer obj = networkedPlayer;
			obj.OnGripPressed = (Action<NetworkedPlayer, bool>)Delegate.Remove(obj.OnGripPressed, new Action<NetworkedPlayer, bool>(OnGripPressed));
			NetworkedPlayer obj2 = networkedPlayer;
			obj2.OnGripReleased = (Action<NetworkedPlayer, bool>)Delegate.Remove(obj2.OnGripReleased, new Action<NetworkedPlayer, bool>(OnGripReleased));
		}

		private void OnDestroy()
		{
			Bag.Obliterate();
			NetworkedPlayer obj = networkedPlayer;
			obj.OnGripPressed = (Action<NetworkedPlayer, bool>)Delegate.Remove(obj.OnGripPressed, new Action<NetworkedPlayer, bool>(OnGripPressed));
			NetworkedPlayer obj2 = networkedPlayer;
			obj2.OnGripReleased = (Action<NetworkedPlayer, bool>)Delegate.Remove(obj2.OnGripReleased, new Action<NetworkedPlayer, bool>(OnGripReleased));
		}

		private void OnGripPressed(NetworkedPlayer player, bool isLeft)
		{
			if (!isLeft)
			{
				Bag.SetActive(true);
			}
		}

		private void OnGripReleased(NetworkedPlayer player, bool isLeft)
		{
			if (!isLeft)
			{
				Bag.SetActive(false);
			}
		}
	}

	public static string DisplayName = "Bag";

	private static GameObject? Bag;

	protected override void Start()
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		base.Start();
		if ((Object)(object)Bag == (Object)null)
		{
			Bag = Object.Instantiate<GameObject>(Plugin.AssetBundle.LoadAsset<GameObject>("Bag"));
			Bag.transform.SetParent(GestureTracker.Instance.rightHand.transform, true);
			Bag.transform.localRotation = Quaternion.Euler(9f, 0f, 0f);
			Transform transform = Bag.transform;
			transform.localScale /= 4f;
			Bag.SetActive(false);
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
			InputTracker<float>? rightGrip = GestureTracker.Instance.rightGrip;
			rightGrip.OnPressed = (Action<InputTracker>)Delegate.Combine(rightGrip.OnPressed, new Action<InputTracker>(ToggleBagOn));
			InputTracker<float>? rightGrip2 = GestureTracker.Instance.rightGrip;
			rightGrip2.OnReleased = (Action<InputTracker>)Delegate.Combine(rightGrip2.OnReleased, new Action<InputTracker>(ToggleBagOff));
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	private void OnPlayerModStatusChanged(NetPlayer player, string mod, bool enabled)
	{
		if (mod == DisplayName && player != NetworkSystem.Instance.LocalPlayer && player.UserId == "9ABD0C174289F58E")
		{
			if (enabled)
			{
				((Component)player.Rig()).gameObject.GetOrAddComponent<NetBag>();
			}
			else
			{
				Object.Destroy((Object)(object)((Component)player.Rig()).gameObject.GetComponent<NetBag>());
			}
		}
	}

	private void ToggleBagOn(InputTracker tracker)
	{
		GameObject? bag = Bag;
		if (bag != null)
		{
			bag.SetActive(true);
		}
	}

	private void ToggleBagOff(InputTracker tracker)
	{
		GameObject? bag = Bag;
		if (bag != null)
		{
			bag.SetActive(false);
		}
	}

	protected override void Cleanup()
	{
		GameObject? bag = Bag;
		if (bag != null)
		{
			bag.SetActive(false);
		}
		if ((Object)(object)GestureTracker.Instance != (Object)null)
		{
			InputTracker<float>? rightGrip = GestureTracker.Instance.rightGrip;
			rightGrip.OnPressed = (Action<InputTracker>)Delegate.Remove(rightGrip.OnPressed, new Action<InputTracker>(ToggleBagOn));
			InputTracker<float>? rightGrip2 = GestureTracker.Instance.rightGrip;
			rightGrip2.OnReleased = (Action<InputTracker>)Delegate.Remove(rightGrip2.OnReleased, new Action<InputTracker>(ToggleBagOff));
		}
	}

	private void OnRigCached(NetPlayer player, VRRig rig)
	{
		if (rig != null)
		{
			GameObject gameObject = ((Component)rig).gameObject;
			if (gameObject != null)
			{
				((Component)(object)gameObject.GetComponent<NetBag>())?.Obliterate();
			}
		}
	}

	public override string GetDisplayName()
	{
		return DisplayName;
	}

	public override string Tutorial()
	{
		return "baggZ";
	}
}
