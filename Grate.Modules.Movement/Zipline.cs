using System;
using BepInEx.Configuration;
using GorillaLocomotion;
using GorillaLocomotion.Climbing;
using GorillaLocomotion.Gameplay;
using Grate.Extensions;
using Grate.GUI;
using Grate.Gestures;
using Grate.Tools;
using HarmonyLib;
using UnityEngine;
using UnityEngine.XR;

namespace Grate.Modules.Movement;

public class Zipline : GrateModule
{
	public static readonly string DisplayName = "Zipline";

	public static GameObject launcherPrefab;

	public static GameObject ziplinePrefab;

	public static AudioClip ziplineAudioLoop;

	public static ConfigEntry<int> MaxZiplines;

	public static ConfigEntry<string> LauncherHand;

	public static ConfigEntry<int> GravityMultiplier;

	private AudioSource audioSlide;

	private AudioSource audioFire;

	private GorillaClimbable climbable;

	private Transform climbOffsetHelper;

	private GameObject gunStartHook;

	private GameObject gunEndHook;

	private XRNode hand;

	public GameObject launcher;

	private int nextZipline;

	private GorillaZiplineSettings settings;

	private ParticleSystem[] smokeSystems;

	public GameObject[] ziplines = (GameObject[])(object)new GameObject[0];

	private void Awake()
	{
		settings = ScriptableObject.CreateInstance<GorillaZiplineSettings>();
		settings.gravityMulti = 0f;
		settings.maxFriction = 0f;
		settings.friction = 0f;
		GorillaZiplineSettings obj = settings;
		obj.maxSpeed *= 2f;
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
			if (!Object.op_Implicit((Object)(object)launcherPrefab))
			{
				launcherPrefab = Plugin.AssetBundle.LoadAsset<GameObject>("Zipline Launcher");
				ziplinePrefab = Plugin.AssetBundle.LoadAsset<GameObject>("Zipline Rope");
				ziplineAudioLoop = Plugin.AssetBundle.LoadAsset<AudioClip>("Zipline Loop");
			}
			launcher = Object.Instantiate<GameObject>(launcherPrefab);
			audioFire = launcher.GetComponent<AudioSource>();
			gunStartHook = ((Component)launcher.transform.Find("Start Hook")).gameObject;
			gunEndHook = ((Component)launcher.transform.Find("End Hook")).gameObject;
			ReloadConfiguration();
			smokeSystems = launcher.GetComponentsInChildren<ParticleSystem>();
			ParticleSystem[] array = smokeSystems;
			for (int i = 0; i < array.Length; i++)
			{
				((Component)array[i]).gameObject.SetActive(false);
			}
			HideLauncher();
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	private void ShowLauncher(InputTracker _)
	{
		((Renderer)launcher.GetComponent<MeshRenderer>()).enabled = true;
		ResetHooks();
	}

	private void HideLauncher()
	{
		((Renderer)launcher.GetComponent<MeshRenderer>()).enabled = false;
		gunStartHook.SetActive(false);
		gunEndHook.SetActive(false);
	}

	private void Fire(InputTracker _)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Invalid comparison between Unknown and I4
		if (launcher.activeSelf)
		{
			audioFire.Play();
			GestureTracker.Instance.HapticPulse((int)hand == 4, 1f, 0.25f);
			ParticleSystem[] array = smokeSystems;
			foreach (ParticleSystem obj in array)
			{
				((Component)obj).gameObject.SetActive(true);
				obj.Clear();
				obj.Play();
			}
			ziplines[nextZipline]?.Obliterate();
			ziplines[nextZipline] = MakeZipline();
			nextZipline = MathExtensions.Wrap(nextZipline + 1, 0, ziplines.Length);
			gunStartHook.SetActive(false);
			gunEndHook.SetActive(false);
			HideLauncher();
		}
	}

	private void ResetHooks()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Invalid comparison between Unknown and I4
		if (launcher.activeSelf)
		{
			GestureTracker.Instance.HapticPulse((int)hand == 4);
			gunStartHook.SetActive(true);
			gunEndHook.SetActive(true);
		}
	}

	private GameObject MakeZipline()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GameObject val = Object.Instantiate<GameObject>(ziplinePrefab);
			Vector3[] endpoints = GetEndpoints(gunStartHook.transform.position, gunStartHook.transform.up);
			Vector3 val2 = endpoints[0];
			Vector3 val3 = endpoints[1];
			val.transform.position = val2;
			Transform val4 = val.transform.Find("Start Hook");
			Transform val5 = val.transform.Find("End Hook");
			((Component)val4).transform.position = val2;
			((Component)val5).transform.position = val3;
			val4.localScale *= GTPlayer.Instance.scale;
			val5.localScale *= GTPlayer.Instance.scale;
			val4.rotation = gunStartHook.transform.rotation;
			val5.rotation = gunEndHook.transform.rotation;
			LineRenderer component = val.GetComponent<LineRenderer>();
			component.positionCount = 2;
			component.SetPosition(0, val4.GetChild(0).position);
			component.SetPosition(1, val5.GetChild(0).position);
			((Renderer)component).enabled = true;
			component.startWidth = 0.05f * GTPlayer.Instance.scale;
			component.endWidth = 0.05f * GTPlayer.Instance.scale;
			MakeSlideHelper(val.transform);
			Transform val6 = MakeSegments(val2, val3);
			val6.parent = val.transform;
			val6.localPosition = Vector3.zero;
			BezierSpline val7 = val.AddComponent<BezierSpline>();
			val7.Reset();
			Traverse.Create((object)val7).Field("points").SetValue((object)SplinePoints(val.transform, val2, val3));
			climbOffsetHelper = new GameObject("Climb Offset Helper").transform;
			GorillaZipline obj = val.AddComponent<GorillaZipline>();
			climbOffsetHelper.SetParent(val.transform, false);
			Traverse obj2 = Traverse.Create((object)obj);
			obj2.Field("spline").SetValue((object)val7);
			obj2.Field("segmentsRoot").SetValue((object)val6);
			obj2.Field("slideHelper").SetValue((object)climbable);
			obj2.Field("audioSlide").SetValue((object)audioSlide);
			obj2.Field("climbOffsetHelper").SetValue((object)climbOffsetHelper);
			obj2.Field("settings").SetValue((object)settings);
			Vector3 val8 = val3 - val2;
			float magnitude = ((Vector3)(ref val8)).magnitude;
			obj2.Field("ziplineDistance").SetValue((object)magnitude);
			obj2.Field("segmentDistance").SetValue((object)magnitude);
			return val;
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
		return null;
	}

	private Vector3[] SplinePoints(Transform parent, Vector3 start, Vector3 end)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		return (Vector3[])(object)new Vector3[4]
		{
			parent.InverseTransformPoint(start),
			parent.InverseTransformPoint(start + (end - start) / 4f),
			parent.InverseTransformPoint(end - (end - start) / 4f),
			parent.InverseTransformPoint(end)
		};
	}

	private Vector3[] GetEndpoints(Vector3 origin, Vector3 forward)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		Ray val = default(Ray);
		((Ray)(ref val))._002Ector(origin, forward);
		RaycastHit val2 = default(RaycastHit);
		Physics.Raycast(val, ref val2, float.PositiveInfinity, Teleport.layerMask);
		if (!Object.op_Implicit((Object)(object)((RaycastHit)(ref val2)).collider))
		{
			return null;
		}
		Vector3 point = ((RaycastHit)(ref val2)).point;
		((Ray)(ref val)).direction = ((Ray)(ref val)).direction * -1f;
		Physics.Raycast(val, ref val2, float.PositiveInfinity, Teleport.layerMask);
		if (!Object.op_Implicit((Object)(object)((RaycastHit)(ref val2)).collider))
		{
			return null;
		}
		Vector3 point2 = ((RaycastHit)(ref val2)).point;
		return (Vector3[])(object)new Vector3[2] { point2, point };
	}

	private void MakeSlideHelper(Transform parent)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		GameObject val = new GameObject("SlideHelper");
		val.transform.SetParent(parent, false);
		val.AddComponent<GorillaSurfaceOverride>().overrideIndex = 89;
		climbable = val.AddComponent<GorillaClimbable>();
		climbable.snapX = true;
		climbable.snapY = true;
		climbable.snapZ = true;
		audioSlide = val.AddComponent<AudioSource>();
		audioSlide.clip = ziplineAudioLoop;
	}

	private Transform MakeSegments(Vector3 start, Vector3 end)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = end - start;
		_ = ((Vector3)(ref val)).magnitude;
		Transform transform = new GameObject("Segments").transform;
		transform.position = start;
		Vector3 position = Vector3.Lerp(start, end, 0.5f);
		MakeSegment(position, start, end).transform.SetParent(transform);
		return transform;
	}

	private GameObject MakeSegment(Vector3 position, Vector3 start, Vector3 end)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		GameObject obj = GameObject.CreatePrimitive((PrimitiveType)3);
		obj.transform.position = position;
		obj.AddComponent<GorillaClimbableRef>().climb = climbable;
		((Collider)obj.GetComponent<BoxCollider>()).isTrigger = true;
		obj.layer = LayerMask.NameToLayer("GorillaInteractable");
		Vector3 val = end - start;
		float magnitude = ((Vector3)(ref val)).magnitude;
		obj.transform.localScale = new Vector3(0.05f * GTPlayer.Instance.scale, 0.05f * GTPlayer.Instance.scale, magnitude);
		obj.transform.LookAt(end, Vector3.up);
		((Renderer)obj.GetComponent<MeshRenderer>()).enabled = false;
		return obj;
	}

	protected override void Cleanup()
	{
		if (!MenuController.Instance.Built)
		{
			return;
		}
		UnsubscribeFromEvents();
		launcher?.Obliterate();
		GameObject obj = gunStartHook;
		if (obj != null)
		{
			obj.gameObject?.Obliterate();
		}
		GameObject obj2 = gunEndHook;
		if (obj2 != null)
		{
			obj2.gameObject?.Obliterate();
		}
		if (ziplines != null)
		{
			GameObject[] array = ziplines;
			for (int i = 0; i < array.Length; i++)
			{
				array[i]?.Obliterate();
			}
		}
	}

	protected override void ReloadConfiguration()
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		settings.gravityMulti = (float)GravityMultiplier.Value / 5f;
		ResizeArray(MaxZiplines.Value);
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
		if (newLength < ziplines.Length)
		{
			for (int i = newLength; i < ziplines.Length; i++)
			{
				ziplines[i]?.Obliterate();
			}
		}
		if (nextZipline >= ziplines.Length)
		{
			nextZipline = 0;
		}
		Array.Resize(ref ziplines, newLength);
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
		launcher.transform.localRotation = Quaternion.Euler(20f, 0f, 180f);
		launcher.transform.localScale = Vector3.one * 18f;
	}

	private void UnsubscribeFromEvents()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		if (Object.op_Implicit((Object)(object)GestureTracker.Instance))
		{
			GestureTracker.Instance.GetInputTracker("grip", hand);
			InputTracker? inputTracker = GestureTracker.Instance.GetInputTracker("trigger", hand);
			inputTracker.OnPressed = (Action<InputTracker>)Delegate.Remove(inputTracker.OnPressed, new Action<InputTracker>(ShowLauncher));
			inputTracker.OnReleased = (Action<InputTracker>)Delegate.Remove(inputTracker.OnReleased, new Action<InputTracker>(Fire));
		}
	}

	public static void BindConfigEntries()
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		MaxZiplines = Plugin.ConfigFile.Bind<int>(DisplayName, "max ziplines", 3, "Maximum number of ziplines that can exist at one time");
		LauncherHand = Plugin.ConfigFile.Bind<string>(DisplayName, "launcher hand", "right", new ConfigDescription("Which hand holds the launcher", (AcceptableValueBase)(object)new AcceptableValueList<string>(new string[2] { "left", "right" }), Array.Empty<object>()));
		GravityMultiplier = Plugin.ConfigFile.Bind<int>(DisplayName, "gravity multiplier", 5, "Gravity multiplier while on the zipline");
	}

	public override string GetDisplayName()
	{
		return DisplayName;
	}

	public override string Tutorial()
	{
		string text = LauncherHand.Value.Substring(0, 1).ToUpper() + LauncherHand.Value.Substring(1);
		return "Hold [" + text + " Trigger] to summon the zipline cannon. Release [" + text + " Trigger] to fire a zipline.";
	}
}
