using System;
using BepInEx.Configuration;
using GorillaLocomotion;
using Grate.GUI;
using Grate.Tools;
using UnityEngine;

namespace Grate.Modules;

public class SpeedBoost : GrateModule
{
	public static readonly string DisplayName = "Speed Boost";

	public static float baseVelocityLimit;

	public static float scale = 1.5f;

	public static bool active;

	public static ConfigEntry<int> Speed;

	private void FixedUpdate()
	{
		string text = "";
		try
		{
			text = "Getting Gamemode\n";
			GorillaGameManager instance = GorillaGameManager.instance;
			string text2 = ((instance != null) ? instance.GameModeName() : null);
			text = "Checking status\n";
			if (active)
			{
				switch (text2)
				{
				case null:
				case "NONE":
				case "CASUAL":
					text = "Setting multiplier\n";
					GTPlayer.Instance.jumpMultiplier = 1.3f * scale;
					GTPlayer.Instance.maxJumpSpeed = 8.5f * scale;
					break;
				}
			}
		}
		catch (Exception e)
		{
			Logging.Debug("GorillaGameManager.instance is null:", GorillaGameManager.instance == null);
			object[] obj = new object[2] { "GorillaGameManager.instance.GameMode() is null:", null };
			GorillaGameManager instance2 = GorillaGameManager.instance;
			obj[1] = ((instance2 != null) ? instance2.GameModeName() : null) == null;
			Logging.Debug(obj);
			Logging.Debug(text);
			Logging.Exception(e);
		}
	}

	protected override void OnEnable()
	{
		if (MenuController.Instance.Built)
		{
			base.OnEnable();
			active = true;
			baseVelocityLimit = GTPlayer.Instance.velocityLimit;
			ReloadConfiguration();
		}
	}

	protected override void Cleanup()
	{
		if (active)
		{
			scale = 1f;
			GTPlayer.Instance.velocityLimit = baseVelocityLimit;
			active = false;
		}
	}

	protected override void ReloadConfiguration()
	{
		scale = 1f + (float)Speed.Value / 20f;
		if (((Behaviour)this).enabled)
		{
			GTPlayer.Instance.velocityLimit = baseVelocityLimit * scale;
		}
	}

	public static void BindConfigEntries()
	{
		Speed = Plugin.ConfigFile.Bind<int>(DisplayName, "speed", 5, "How fast you run while speed boost is active");
	}

	public override string GetDisplayName()
	{
		return DisplayName;
	}

	public override string Tutorial()
	{
		return "Effect: Increases your jump strength.";
	}
}
