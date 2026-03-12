using System;
using BepInEx.Configuration;
using Grate.Extensions;
using Grate.Modules.Physics;
using Grate.Networking;
using Grate.Tools;
using UnityEngine;

namespace Grate.Modules.Multiplayer;

internal class NetworkedKaemeManager : MonoBehaviour
{
	private LineRenderer? bananaLine;

	private ParticleSystem? Effects;

	private Color khameColor;

	public NetworkedPlayer? networkedPlayer;

	private Transform? orb;

	private string state = "";

	public ConfigEntry<bool>? IsNetworked { get; }

	private void Start()
	{
		try
		{
			networkedPlayer = ((Component)this).gameObject.GetComponent<NetworkedPlayer>();
			orb = Object.Instantiate<Transform>(Kamehameha.orb);
			ComponentUtils.AddComponent<RoomSpecific>((Component)(object)orb).Owner = networkedPlayer.owner;
			bananaLine = Object.Instantiate<LineRenderer>(Kamehameha.bananaLine);
			ComponentUtils.AddComponent<RoomSpecific>((Component)(object)bananaLine).Owner = networkedPlayer.owner;
			((Component)bananaLine).gameObject.SetActive(true);
			((Object)orb).name = networkedPlayer.owner.NickName + "s Orb";
			((Object)bananaLine).name = networkedPlayer.owner.NickName + "s Line";
			Effects = ((Component)orb).GetComponentInChildren<ParticleSystem>();
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	private void FixedUpdate()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			khameColor = networkedPlayer.owner.GetProperty<string>(Kamehameha.KamehamehaColorKey).StringToColor();
			((Component)orb).GetComponent<Renderer>().material.color = khameColor;
			bananaLine.SetColors(khameColor, khameColor);
			((Component)Effects).GetComponent<Renderer>().material.color = khameColor;
			state = networkedPlayer.owner.GetProperty<string>(Kamehameha.KamehamehaKey);
		}
		catch (Exception ex)
		{
			Debug.LogError((object)ex.Message);
			Debug.LogError((object)ex.Source);
			Debug.LogError((object)ex.StackTrace);
			Object.Destroy((Object)(object)this);
		}
		switch (state)
		{
		case "None":
			((Component)orb).gameObject.SetActive(false);
			((Renderer)bananaLine).forceRenderingOff = true;
			break;
		case "Charging":
			((Component)orb).gameObject.SetActive(true);
			((Renderer)bananaLine).forceRenderingOff = true;
			HandleStuff();
			break;
		case "FIRE!":
			((Component)orb).gameObject.SetActive(true);
			((Renderer)bananaLine).forceRenderingOff = false;
			HandleStuff();
			break;
		}
	}

	private void OnDestroy()
	{
		if ((Object)(object)orb != (Object)null)
		{
			Transform? obj = orb;
			if (obj != null)
			{
				((Component)obj).gameObject.Obliterate();
			}
			LineRenderer? obj2 = bananaLine;
			if (obj2 != null)
			{
				((Component)obj2).gameObject.Obliterate();
			}
		}
	}

	private void HandleStuff()
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		Transform leftHandTransform = networkedPlayer.rig.leftHandTransform;
		Transform rightHandTransform = networkedPlayer.rig.rightHandTransform;
		float num = 0f;
		float num2 = networkedPlayer.owner.GetProperty<float>(Potions.playerSizeKey) / 2f;
		num = Vector3.Distance(leftHandTransform.position, rightHandTransform.position);
		num = Mathf.Clamp(num, 0f, Kamehameha.maxOrbSize * num2 * 2f);
		bananaLine.startWidth = num * num2;
		bananaLine.endWidth = num * num2;
		Vector3 val = (leftHandTransform.right + rightHandTransform.right * -1f) / 2f;
		Vector3 val2 = (leftHandTransform.position + rightHandTransform.position) / 2f + val * 0.1f;
		orb.position = val2;
		((Component)orb).transform.localScale = Vector3.one * num * num2;
		bananaLine.SetPosition(0, val2);
		bananaLine.SetPosition(1, val2 - val * 100f);
	}
}
