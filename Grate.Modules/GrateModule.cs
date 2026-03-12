using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx.Configuration;
using Grate.Networking;
using Grate.Tools;
using UnityEngine;

namespace Grate.Modules;

public abstract class GrateModule : MonoBehaviour
{
	public static GrateModule LastEnabled;

	public static Dictionary<string, bool> enabledModules = new Dictionary<string, bool>();

	public static string enabledModulesKey = "GrateEnabledModules";

	public ButtonController button;

	public List<ConfigEntryBase> ConfigEntries;

	protected virtual void Start()
	{
		((Behaviour)this).enabled = false;
	}

	protected virtual void OnEnable()
	{
		LastEnabled = this;
		Plugin.ConfigFile.SettingChanged += SettingsChanged;
		if (Object.op_Implicit((Object)(object)button))
		{
			button.IsPressed = true;
		}
		SetStatus(enabled: true);
	}

	protected virtual void OnDisable()
	{
		Plugin.ConfigFile.SettingChanged -= SettingsChanged;
		if (Object.op_Implicit((Object)(object)button))
		{
			button.IsPressed = false;
		}
		Cleanup();
		SetStatus(enabled: false);
	}

	protected virtual void OnDestroy()
	{
		Cleanup();
	}

	protected virtual void ReloadConfiguration()
	{
	}

	public abstract string GetDisplayName();

	protected void SettingsChanged(object sender, SettingChangedEventArgs e)
	{
		FieldInfo[] fields = ((object)this).GetType().GetFields();
		foreach (FieldInfo fieldInfo in fields)
		{
			if (e.ChangedSetting == fieldInfo.GetValue(this))
			{
				ReloadConfiguration();
			}
		}
	}

	public abstract string Tutorial();

	protected abstract void Cleanup();

	public void SetStatus(bool enabled)
	{
		string displayName = GetDisplayName();
		if (enabledModules.ContainsKey(displayName))
		{
			enabledModules[displayName] = enabled;
		}
		else
		{
			enabledModules.Add(displayName, enabled);
		}
		NetworkPropertyHandler.Instance?.ChangeProperty(enabledModulesKey, enabledModules);
	}

	public static List<Type> GetGrateModuleTypes()
	{
		try
		{
			List<Type> list = (from t in Assembly.GetExecutingAssembly().GetTypes()
				where typeof(GrateModule).IsAssignableFrom(t)
				select t).ToList();
			list.Sort(delegate(Type x, Type y)
			{
				FieldInfo field = x.GetField("DisplayName", BindingFlags.Static | BindingFlags.Public);
				FieldInfo field2 = y.GetField("DisplayName", BindingFlags.Static | BindingFlags.Public);
				if (field == null || field2 == null)
				{
					return 0;
				}
				string strA = (string)field.GetValue(null);
				string strB = (string)field2.GetValue(null);
				return string.Compare(strA, strB);
			});
			return list;
		}
		catch (ReflectionTypeLoadException ex)
		{
			Logging.Exception(ex);
			Logging.Warning("Inner exceptions:");
			Exception[] loaderExceptions = ex.LoaderExceptions;
			for (int i = 0; i < loaderExceptions.Length; i++)
			{
				Logging.Exception(loaderExceptions[i]);
			}
		}
		return null;
	}
}
