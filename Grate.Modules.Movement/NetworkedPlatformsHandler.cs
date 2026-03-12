using System;
using Grate.Extensions;
using Grate.Networking;
using Grate.Tools;
using UnityEngine;

namespace Grate.Modules.Movement;

public class NetworkedPlatformsHandler : MonoBehaviour
{
	public NetworkedPlayer? networkedPlayer;

	public GameObject? platformLeft;

	public GameObject? platformRight;

	private void Start()
	{
		try
		{
			networkedPlayer = ((Component)this).gameObject.GetComponent<NetworkedPlayer>();
			Logging.Debug("Networked player", networkedPlayer.owner.NickName, "turned on platforms");
			platformLeft = Object.Instantiate<GameObject>(Platforms.platformPrefab);
			platformRight = Object.Instantiate<GameObject>(Platforms.platformPrefab);
			SetupPlatform(platformLeft);
			SetupPlatform(platformRight);
			((Object)platformLeft).name = networkedPlayer.owner.NickName + "'s Left Platform";
			((Object)platformRight).name = networkedPlayer.owner.NickName + "'s Right Platform";
			NetworkedPlayer? obj = networkedPlayer;
			obj.OnGripPressed = (Action<NetworkedPlayer, bool>)Delegate.Combine(obj.OnGripPressed, new Action<NetworkedPlayer, bool>(OnGripPressed));
			NetworkedPlayer? obj2 = networkedPlayer;
			obj2.OnGripReleased = (Action<NetworkedPlayer, bool>)Delegate.Combine(obj2.OnGripReleased, new Action<NetworkedPlayer, bool>(OnGripReleased));
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	private void OnDestroy()
	{
		platformLeft?.Obliterate();
		platformRight?.Obliterate();
		if ((Object)(object)networkedPlayer != (Object)null)
		{
			NetworkedPlayer? obj = networkedPlayer;
			obj.OnGripPressed = (Action<NetworkedPlayer, bool>)Delegate.Remove(obj.OnGripPressed, new Action<NetworkedPlayer, bool>(OnGripPressed));
			NetworkedPlayer? obj2 = networkedPlayer;
			obj2.OnGripReleased = (Action<NetworkedPlayer, bool>)Delegate.Remove(obj2.OnGripReleased, new Action<NetworkedPlayer, bool>(OnGripReleased));
		}
	}

	private void SetupPlatform(GameObject platform)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		try
		{
			platform.SetActive(false);
			platform.AddComponent<RoomSpecific>().Owner = networkedPlayer?.owner;
			foreach (Transform item in platform.transform)
			{
				Transform val = item;
				if (!((Object)val).name.Contains("cloud"))
				{
					((Component)val).gameObject.Obliterate();
					continue;
				}
				((Component)(object)((Component)val).GetComponent<Collider>())?.Obliterate();
				((Component)(object)((Component)val).GetComponent<ParticleSystem>())?.Obliterate();
			}
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	private void OnGripPressed(NetworkedPlayer player, bool isLeft)
	{
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		if (isLeft)
		{
			Transform leftHandTransform = networkedPlayer.rig.leftHandTransform;
			platformLeft.SetActive(true);
			platformLeft.transform.position = leftHandTransform.TransformPoint(new Vector3(-12f, 18f, -10f) / 200f);
			platformLeft.transform.rotation = ((Component)leftHandTransform).transform.rotation * Quaternion.Euler(215f, 0f, -15f);
			platformLeft.transform.localScale = Vector3.one * networkedPlayer.rig.scaleFactor;
		}
		else
		{
			Transform rightHandTransform = networkedPlayer.rig.rightHandTransform;
			platformRight.SetActive(true);
			platformRight.transform.localPosition = rightHandTransform.TransformPoint(new Vector3(12f, 18f, 10f) / 200f);
			platformRight.transform.localRotation = ((Component)rightHandTransform).transform.rotation * Quaternion.Euler(-45f, -25f, -190f);
			platformLeft.transform.localScale = Vector3.one * networkedPlayer.rig.scaleFactor;
		}
	}

	private void OnGripReleased(NetworkedPlayer player, bool isLeft)
	{
		if (isLeft)
		{
			platformLeft.SetActive(false);
		}
		else
		{
			platformRight.SetActive(false);
		}
	}
}
