using System;
using BepInEx.Configuration;
using GorillaLocomotion;
using Grate.Extensions;
using Grate.GUI;
using Grate.Tools;
using UnityEngine;

namespace Grate.Modules.Movement;

public class GrapplingHooks : GrateModule
{
	public static readonly string DisplayName = "Grappling Hooks";

	public static ConfigEntry<int> Spring;

	public static ConfigEntry<int> Steering;

	public static ConfigEntry<int> MaxLength;

	public static ConfigEntry<string> RopeType;

	private readonly Vector3 holsterOffset = new Vector3(0.15f, -0.15f, 0.15f);

	private GameObject bananaGunPrefab;

	private GameObject bananaGunL;

	private GameObject bananaGunR;

	private Transform holsterL;

	private Transform holsterR;

	private void Awake()
	{
		try
		{
			bananaGunPrefab = Plugin.AssetBundle.LoadAsset<GameObject>("Banana Gun");
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
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!Object.op_Implicit((Object)(object)bananaGunPrefab))
			{
				bananaGunPrefab = Plugin.AssetBundle.LoadAsset<GameObject>("Banana Gun");
			}
			holsterL = new GameObject("Holster (Left)").transform;
			bananaGunL = Object.Instantiate<GameObject>(bananaGunPrefab);
			SetupBananaGun(ref holsterL, ref bananaGunL, isLeft: true);
			holsterR = new GameObject("Holster (Right)").transform;
			bananaGunR = Object.Instantiate<GameObject>(bananaGunPrefab);
			SetupBananaGun(ref holsterR, ref bananaGunR, isLeft: false);
			ReloadConfiguration();
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	private void SetupBananaGun(ref Transform holster, ref GameObject bananaGun, bool isLeft)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			holster.SetParent(((Component)GTPlayer.Instance.bodyCollider).transform, false);
			Vector3 localPosition = default(Vector3);
			((Vector3)(ref localPosition))._002Ector(holsterOffset.x * (float)((!isLeft) ? 1 : (-1)), holsterOffset.y, holsterOffset.z);
			holster.localPosition = localPosition;
			BananaGun bananaGun2 = bananaGun.AddComponent<BananaGun>();
			((Object)bananaGun2).name = (isLeft ? "Banana Grapple Left" : "Banana Grapple Right");
			bananaGun2.Holster(holster);
			bananaGun2.SetupInteraction();
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	protected override void Cleanup()
	{
		try
		{
			Transform obj = holsterL;
			if (obj != null)
			{
				((Component)obj).gameObject?.Obliterate();
			}
			Transform obj2 = holsterR;
			if (obj2 != null)
			{
				((Component)obj2).gameObject?.Obliterate();
			}
			GameObject obj3 = bananaGunL;
			if (obj3 != null)
			{
				obj3.gameObject?.Obliterate();
			}
			GameObject obj4 = bananaGunR;
			if (obj4 != null)
			{
				obj4.gameObject?.Obliterate();
			}
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	protected override void ReloadConfiguration()
	{
		BananaGun[] array = new BananaGun[2];
		GameObject obj = bananaGunL;
		array[0] = ((obj != null) ? obj.GetComponent<BananaGun>() : null);
		GameObject obj2 = bananaGunR;
		array[1] = ((obj2 != null) ? obj2.GetComponent<BananaGun>() : null);
		BananaGun[] array2 = array;
		foreach (BananaGun bananaGun in array2)
		{
			if (Object.op_Implicit((Object)(object)bananaGun))
			{
				bananaGun.pullForce = Spring.Value * 2;
				bananaGun.ropeType = ((!(RopeType.Value == "elastic")) ? BananaGun.RopeType.STATIC : BananaGun.RopeType.ELASTIC);
				bananaGun.steerForce = (float)Steering.Value / 2f;
				bananaGun.maxLength = MaxLength.Value * 5;
				Logging.Debug("gun.pullForce:", bananaGun.pullForce, "gun.ropeType:", bananaGun.ropeType, "gun.steerForce:", bananaGun.steerForce, "gun.maxLength:", bananaGun.maxLength);
			}
		}
	}

	public static void BindConfigEntries()
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		RopeType = Plugin.ConfigFile.Bind<string>(DisplayName, "rope type", "elastic", new ConfigDescription("Whether the rope should pull you to the anchor point or not", (AcceptableValueBase)(object)new AcceptableValueList<string>(new string[2] { "elastic", "rope" }), Array.Empty<object>()));
		Spring = Plugin.ConfigFile.Bind<int>(DisplayName, "springiness", 5, "If ropes are elastic, this is how springy the ropes are");
		Steering = Plugin.ConfigFile.Bind<int>(DisplayName, "steering", 5, "How much influence you have over your velocity");
		MaxLength = Plugin.ConfigFile.Bind<int>(DisplayName, "max length", 5, "The maximum distance that the grappling hook can reach");
	}

	public override string GetDisplayName()
	{
		return DisplayName;
	}

	public override string Tutorial()
	{
		return "Grab the grappling hook off of your waist with [Grip]. Then fire with [Trigger]. You can steer in the air by pointing the guns where you want to go.";
	}
}
