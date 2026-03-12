using System;
using Grate.Extensions;
using Grate.GUI;
using Grate.Gestures;
using Grate.Modules.Movement;
using Grate.Networking;
using Grate.Patches;
using Grate.Tools;
using UnityEngine;

namespace Grate.Modules.Misc;

public class RatSword : GrateModule
{
	private class NetSword : MonoBehaviour
	{
		private NetworkedPlayer networkedPlayer;

		private GameObject sword;

		private void OnEnable()
		{
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			networkedPlayer = ((Component)this).gameObject.GetComponent<NetworkedPlayer>();
			Transform rightHandTransform = networkedPlayer.rig.rightHandTransform;
			sword = Object.Instantiate<GameObject>(Sword);
			sword.transform.SetParent(rightHandTransform);
			sword.transform.localPosition = new Vector3(0.04f, 0.05f, -0.02f);
			sword.transform.localRotation = Quaternion.Euler(78.4409f, 0f, 0f);
			sword.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
			NetworkedPlayer obj = networkedPlayer;
			obj.OnGripPressed = (Action<NetworkedPlayer, bool>)Delegate.Combine(obj.OnGripPressed, new Action<NetworkedPlayer, bool>(OnGripPressed));
			NetworkedPlayer obj2 = networkedPlayer;
			obj2.OnGripReleased = (Action<NetworkedPlayer, bool>)Delegate.Combine(obj2.OnGripReleased, new Action<NetworkedPlayer, bool>(OnGripReleased));
		}

		private void OnDisable()
		{
			sword.Obliterate();
			NetworkedPlayer obj = networkedPlayer;
			obj.OnGripPressed = (Action<NetworkedPlayer, bool>)Delegate.Remove(obj.OnGripPressed, new Action<NetworkedPlayer, bool>(OnGripPressed));
			NetworkedPlayer obj2 = networkedPlayer;
			obj2.OnGripReleased = (Action<NetworkedPlayer, bool>)Delegate.Remove(obj2.OnGripReleased, new Action<NetworkedPlayer, bool>(OnGripReleased));
		}

		private void OnDestroy()
		{
			sword.Obliterate();
			NetworkedPlayer obj = networkedPlayer;
			obj.OnGripPressed = (Action<NetworkedPlayer, bool>)Delegate.Remove(obj.OnGripPressed, new Action<NetworkedPlayer, bool>(OnGripPressed));
			NetworkedPlayer obj2 = networkedPlayer;
			obj2.OnGripReleased = (Action<NetworkedPlayer, bool>)Delegate.Remove(obj2.OnGripReleased, new Action<NetworkedPlayer, bool>(OnGripReleased));
		}

		private void OnGripPressed(NetworkedPlayer player, bool isLeft)
		{
			if (!isLeft)
			{
				sword.SetActive(true);
			}
		}

		private void OnGripReleased(NetworkedPlayer player, bool isLeft)
		{
			if (!isLeft)
			{
				sword.SetActive(false);
			}
		}
	}

	private static readonly string DisplayName = "Rat Sword";

	private static GameObject? Sword;

	protected override void Start()
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		base.Start();
		if ((Object)(object)Sword == (Object)null)
		{
			Sword = Object.Instantiate<GameObject>(Plugin.AssetBundle.LoadAsset<GameObject>("Rat Sword"));
			Sword.transform.SetParent(GestureTracker.Instance.rightHand.transform, true);
			Sword.transform.localPosition = new Vector3(-0.4782f, 0.1f, 0.4f);
			Sword.transform.localRotation = Quaternion.Euler(9f, 0f, 0f);
			Transform transform = Sword.transform;
			transform.localScale /= 2f;
			Sword.SetActive(false);
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
			rightGrip.OnPressed = (Action<InputTracker>)Delegate.Combine(rightGrip.OnPressed, new Action<InputTracker>(ToggleRatSwordOn));
			InputTracker<float>? rightGrip2 = GestureTracker.Instance.rightGrip;
			rightGrip2.OnReleased = (Action<InputTracker>)Delegate.Combine(rightGrip2.OnReleased, new Action<InputTracker>(ToggleRatSwordOff));
			((Behaviour)((Component)Plugin.MenuController).GetComponent<DoubleJump>()).enabled = true;
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	private void OnPlayerModStatusChanged(NetPlayer player, string mod, bool modEnabled)
	{
		if (mod == DisplayName && player != NetworkSystem.Instance.LocalPlayer)
		{
			if (modEnabled)
			{
				((Component)player.Rig()).gameObject.GetOrAddComponent<NetSword>();
			}
			else
			{
				Object.Destroy((Object)(object)((Component)player.Rig()).gameObject.GetComponent<NetSword>());
			}
		}
	}

	private void ToggleRatSwordOn(InputTracker tracker)
	{
		GameObject? sword = Sword;
		if (sword != null)
		{
			sword.SetActive(true);
		}
	}

	private void ToggleRatSwordOff(InputTracker tracker)
	{
		GameObject? sword = Sword;
		if (sword != null)
		{
			sword.SetActive(false);
		}
	}

	protected override void Cleanup()
	{
		GameObject? sword = Sword;
		if (sword != null)
		{
			sword.SetActive(false);
		}
		if ((Object)(object)GestureTracker.Instance != (Object)null)
		{
			InputTracker<float>? rightGrip = GestureTracker.Instance.rightGrip;
			rightGrip.OnPressed = (Action<InputTracker>)Delegate.Remove(rightGrip.OnPressed, new Action<InputTracker>(ToggleRatSwordOn));
			InputTracker<float>? rightGrip2 = GestureTracker.Instance.rightGrip;
			rightGrip2.OnReleased = (Action<InputTracker>)Delegate.Remove(rightGrip2.OnReleased, new Action<InputTracker>(ToggleRatSwordOff));
			((Behaviour)((Component)Plugin.MenuController).GetComponent<DoubleJump>()).enabled = false;
		}
	}

	private void OnRigCached(NetPlayer player, VRRig rig)
	{
		if (rig != null)
		{
			GameObject gameObject = ((Component)rig).gameObject;
			if (gameObject != null)
			{
				((Component)(object)gameObject.GetComponent<NetSword>())?.Obliterate();
			}
		}
	}

	public override string GetDisplayName()
	{
		return DisplayName;
	}

	public override string Tutorial()
	{
		return "I met a lil' kid in canyons who wanted kyle to make him a sword.\n[Grip] to wield your weapon, rat kid.";
	}
}
