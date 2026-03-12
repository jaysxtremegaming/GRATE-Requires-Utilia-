using System;
using BepInEx.Configuration;
using GorillaLocomotion.Climbing;
using Grate.Extensions;
using Grate.GUI;
using Grate.Gestures;
using Grate.Tools;
using UnityEngine;
using UnityEngine.XR;

namespace Grate.Modules.Movement;

public class NailGun : GrateModule
{
	public static readonly string DisplayName = "Nail Gun";

	public static GameObject launcherPrefab;

	public static GameObject nailPrefab;

	public static ConfigEntry<int> MaxNailGuns;

	public static ConfigEntry<string> LauncherHand;

	public static ConfigEntry<int> GravityMultiplier;

	private AudioSource audioFire;

	private GameObject barrel;

	private XRNode hand;

	public GameObject launcher;

	public GameObject[] nails = (GameObject[])(object)new GameObject[0];

	private int nextNail;

	protected override void OnEnable()
	{
		if (!MenuController.Instance.Built)
		{
			return;
		}
		base.OnEnable();
		try
		{
			if (!Object.op_Implicit((Object)(object)launcherPrefab))
			{
				launcherPrefab = Plugin.AssetBundle.LoadAsset<GameObject>("Nail Gun");
				nailPrefab = Plugin.AssetBundle.LoadAsset<GameObject>("Nail");
			}
			launcher = Object.Instantiate<GameObject>(launcherPrefab);
			audioFire = launcher.GetComponent<AudioSource>();
			barrel = ((Component)launcher.transform.Find("Barrel")).gameObject;
			ReloadConfiguration();
			HideLauncher();
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	private void ShowLauncher(InputTracker _)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Invalid comparison between Unknown and I4
		((Renderer)launcher.GetComponent<MeshRenderer>()).enabled = true;
		GestureTracker.Instance.HapticPulse((int)hand == 4);
	}

	private void HideLauncher()
	{
		((Renderer)launcher.GetComponent<MeshRenderer>()).enabled = false;
	}

	private void Fire(InputTracker _)
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Invalid comparison between Unknown and I4
		if (launcher.activeSelf)
		{
			audioFire.Play();
			try
			{
				nails[nextNail]?.Obliterate();
				nails[nextNail] = MakeNail();
				nextNail = MathExtensions.Wrap(nextNail + 1, 0, nails.Length);
				GestureTracker.Instance.HapticPulse((int)hand == 4, 1f, 0.25f);
			}
			catch (Exception e)
			{
				Logging.Exception(e);
			}
			HideLauncher();
		}
	}

	private GameObject MakeNail()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GameObject val = Object.Instantiate<GameObject>(nailPrefab);
			Vector3? endpoint = GetEndpoint(barrel.transform.position, barrel.transform.forward);
			if (!endpoint.HasValue)
			{
				return null;
			}
			val.transform.position = endpoint.Value;
			val.transform.rotation = barrel.transform.rotation;
			val.AddComponent<GorillaClimbable>();
			return val;
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
		return null;
	}

	private Vector3? GetEndpoint(Vector3 origin, Vector3 forward)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		RaycastHit val = default(RaycastHit);
		Physics.Raycast(new Ray(origin, forward), ref val, float.PositiveInfinity, Teleport.layerMask);
		if (!Object.op_Implicit((Object)(object)((RaycastHit)(ref val)).collider))
		{
			return null;
		}
		return ((RaycastHit)(ref val)).point;
	}

	protected override void Cleanup()
	{
		if (!MenuController.Instance.Built)
		{
			return;
		}
		UnsubscribeFromEvents();
		launcher?.Obliterate();
		if (nails != null)
		{
			GameObject[] array = nails;
			for (int i = 0; i < array.Length; i++)
			{
				array[i]?.Obliterate();
			}
		}
	}

	protected override void ReloadConfiguration()
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		ResizeArray(MaxNailGuns.Value * 4);
		UnsubscribeFromEvents();
		hand = (XRNode)((LauncherHand.Value == "left") ? 4 : 5);
		Parent();
		GestureTracker.Instance.GetInputTracker("grip", hand);
		InputTracker? inputTracker = GestureTracker.Instance.GetInputTracker("trigger", hand);
		inputTracker.OnPressed = (Action<InputTracker>)Delegate.Combine(inputTracker.OnPressed, new Action<InputTracker>(ShowLauncher));
		inputTracker.OnReleased = (Action<InputTracker>)Delegate.Combine(inputTracker.OnReleased, new Action<InputTracker>(Fire));
	}

	public void ResizeArray(int newLength)
	{
		if (newLength < 0)
		{
			Logging.Warning("Cannot resize array to a negative length.");
			return;
		}
		if (newLength < nails.Length)
		{
			for (int i = newLength; i < nails.Length; i++)
			{
				nails[i]?.Obliterate();
			}
		}
		if (nextNail >= nails.Length)
		{
			nextNail = 0;
		}
		Array.Resize(ref nails, newLength);
	}

	private void Parent()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Invalid comparison between Unknown and I4
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		Transform transform = GestureTracker.Instance.rightHand.transform;
		float num = -1f;
		if ((int)hand == 4)
		{
			transform = GestureTracker.Instance.leftHand.transform;
			num = 1f;
		}
		launcher.transform.SetParent(transform, true);
		launcher.transform.localPosition = new Vector3(0.4782f * num, 0.1f, 0.4f);
		launcher.transform.localRotation = Quaternion.Euler(20f, 0f, 0f);
		launcher.transform.localScale = Vector3.one * 18f;
	}

	private void UnsubscribeFromEvents()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		if (Object.op_Implicit((Object)(object)GestureTracker.Instance))
		{
			InputTracker? inputTracker = GestureTracker.Instance.GetInputTracker("trigger", hand);
			inputTracker.OnPressed = (Action<InputTracker>)Delegate.Remove(inputTracker.OnPressed, new Action<InputTracker>(ShowLauncher));
			inputTracker.OnReleased = (Action<InputTracker>)Delegate.Remove(inputTracker.OnReleased, new Action<InputTracker>(Fire));
		}
	}

	public static void BindConfigEntries()
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		MaxNailGuns = Plugin.ConfigFile.Bind<int>(DisplayName, "max nails", 5, "Maximum number of nails that can exist at one time (multiplied by 4)");
		LauncherHand = Plugin.ConfigFile.Bind<string>(DisplayName, "nailgun hand", "left", new ConfigDescription("Which hand holds the nail gun", (AcceptableValueBase)(object)new AcceptableValueList<string>(new string[2] { "left", "right" }), Array.Empty<object>()));
	}

	public override string GetDisplayName()
	{
		return DisplayName;
	}

	public override string Tutorial()
	{
		string text = LauncherHand.Value.Substring(0, 1).ToUpper() + LauncherHand.Value.Substring(1);
		return "Hold [" + text + " Trigger] to summon the nailgun. Release [" + text + " Trigger] to fire a climbable nail. Grip the nail to climb it.";
	}
}
