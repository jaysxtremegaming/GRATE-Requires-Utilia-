using System;
using BepInEx.Configuration;
using Grate.Extensions;
using Grate.GUI;
using Grate.Tools;
using UnityEngine;

namespace Grate.Modules.Movement;

public class Rockets : GrateModule
{
	public static readonly string DisplayName = "Rockets";

	public static Rockets Instance;

	public static ConfigEntry<int> Power;

	public static ConfigEntry<int> Volume;

	private Rocket rocketL;

	private Rocket rocketR;

	private GameObject rocketPrefab;

	private void Awake()
	{
		try
		{
			Instance = this;
			rocketPrefab = Plugin.AssetBundle.LoadAsset<GameObject>("Rocket");
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	protected override void Start()
	{
		base.Start();
	}

	protected override void OnEnable()
	{
		if (MenuController.Instance.Built)
		{
			base.OnEnable();
			Setup();
		}
	}

	private void Setup()
	{
		try
		{
			if (!Object.op_Implicit((Object)(object)rocketPrefab))
			{
				rocketPrefab = Plugin.AssetBundle.LoadAsset<GameObject>("Rocket");
			}
			rocketL = SetupRocket(Object.Instantiate<GameObject>(rocketPrefab), isLeft: true);
			rocketR = SetupRocket(Object.Instantiate<GameObject>(rocketPrefab), isLeft: false);
			ReloadConfiguration();
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	private Rocket SetupRocket(GameObject rocketObj, bool isLeft)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			((Object)rocketObj).name = (isLeft ? "Grate Rocket Left" : "Grate Rocket Right");
			Rocket rocket = rocketObj.AddComponent<Rocket>().Init(isLeft);
			rocket.LocalPosition = new Vector3(0.51f, -3f, 0f);
			rocket.LocalRotation = new Vector3(0f, 0f, -90f);
			return rocket;
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
		return null;
	}

	public Vector3 AddedVelocity()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		return rocketL.force + rocketR.force;
	}

	protected override void Cleanup()
	{
		try
		{
			Rocket rocket = rocketL;
			if (rocket != null)
			{
				((Component)rocket).gameObject?.Obliterate();
			}
			Rocket rocket2 = rocketR;
			if (rocket2 != null)
			{
				((Component)rocket2).gameObject?.Obliterate();
			}
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	protected override void ReloadConfiguration()
	{
		Rocket[] array = new Rocket[2];
		Rocket rocket = rocketL;
		array[0] = ((rocket != null) ? ((Component)rocket).GetComponent<Rocket>() : null);
		Rocket rocket2 = rocketR;
		array[1] = ((rocket2 != null) ? ((Component)rocket2).GetComponent<Rocket>() : null);
		Rocket[] array2 = array;
		foreach (Rocket rocket3 in array2)
		{
			if (Object.op_Implicit((Object)(object)rocket3))
			{
				rocket3.power = (float)Power.Value * 2f;
				rocket3.volume = MathExtensions.Map(Volume.Value, 0f, 10f, 0f, 1f);
			}
		}
	}

	public static void BindConfigEntries()
	{
		Power = Plugin.ConfigFile.Bind<int>(DisplayName, "power", 5, "The power of each rocket");
		Volume = Plugin.ConfigFile.Bind<int>(DisplayName, "thruster volume", 10, "How loud the thrusters sound");
	}

	public override string GetDisplayName()
	{
		return DisplayName;
	}

	public override string Tutorial()
	{
		return "Hold either [Grip] to summon a rocket.";
	}
}
