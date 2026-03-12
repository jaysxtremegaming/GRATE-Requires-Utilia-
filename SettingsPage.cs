using System;
using System.Collections.Generic;
using BepInEx.Configuration;
using Grate;
using Grate.Extensions;
using Grate.GUI;
using Grate.Modules;
using Grate.Tools;
using UnityEngine;

public class SettingsPage : MonoBehaviour
{
	private ConfigEntryBase entry;

	private GrateOptionWheel modSelector;

	private GrateOptionWheel configSelector;

	private GrateSlider valueSlider;

	private void Awake()
	{
		try
		{
			modSelector = ((Component)((Component)this).transform.Find("Mod Selector")).gameObject.AddComponent<GrateOptionWheel>();
			modSelector.InitializeValues(GetModulesWithSettings());
			configSelector = ((Component)((Component)this).transform.Find("Config Selector")).gameObject.AddComponent<GrateOptionWheel>();
			configSelector.InitializeValues(GetConfigKeys(modSelector.Selected));
			valueSlider = ((Component)((Component)this).transform.Find("Value Slider")).gameObject.AddComponent<GrateSlider>();
			entry = GetEntry(modSelector.Selected, configSelector.Selected);
			ConfigExtensions.ConfigValueInfo configValueInfo = entry.ValuesInfo();
			valueSlider.InitializeValues(configValueInfo.AcceptableValues, configValueInfo.InitialValue);
			GrateOptionWheel grateOptionWheel = modSelector;
			grateOptionWheel.OnValueChanged = (Action<string>)Delegate.Combine(grateOptionWheel.OnValueChanged, (Action<string>)delegate(string mod)
			{
				configSelector.InitializeValues(GetConfigKeys(mod));
			});
			GrateOptionWheel grateOptionWheel2 = configSelector;
			grateOptionWheel2.OnValueChanged = (Action<string>)Delegate.Combine(grateOptionWheel2.OnValueChanged, (Action<string>)delegate
			{
				entry = GetEntry(modSelector.Selected, configSelector.Selected);
				UpdateText();
				ConfigExtensions.ConfigValueInfo configValueInfo2 = entry.ValuesInfo();
				valueSlider.InitializeValues(configValueInfo2.AcceptableValues, configValueInfo2.InitialValue);
			});
			GrateSlider grateSlider = valueSlider;
			grateSlider.OnValueChanged = (Action<object>)Delegate.Combine(grateSlider.OnValueChanged, (Action<object>)delegate(object value)
			{
				entry.BoxedValue = value;
			});
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	private ConfigEntryBase GetEntry(string modName, string key)
	{
		foreach (ConfigDefinition key2 in Plugin.ConfigFile.Keys)
		{
			if (key2.Section == modName && key2.Key == key)
			{
				return Plugin.ConfigFile[key2];
			}
		}
		throw new Exception("Could not find config entry for " + modName + " with key " + key);
	}

	private List<string> GetConfigKeys(string modName)
	{
		try
		{
			List<string> list = new List<string>();
			foreach (ConfigDefinition key in Plugin.ConfigFile.Keys)
			{
				if (key.Section == modName)
				{
					list.Add(Plugin.ConfigFile[key].Definition.Key);
				}
			}
			return list;
		}
		catch (Exception e)
		{
			Logging.Exception(e);
			return null;
		}
	}

	private List<string> GetModulesWithSettings()
	{
		try
		{
			List<string> list = new List<string> { "General" };
			foreach (Type grateModuleType in GrateModule.GetGrateModuleTypes())
			{
				if (!(grateModuleType == typeof(GrateModule)) && (object)grateModuleType.GetMethod("BindConfigEntries") != null)
				{
					string item = (string)grateModuleType.GetField("DisplayName").GetValue(null);
					list.Add(item);
				}
			}
			return list;
		}
		catch (Exception e)
		{
			Logging.Exception(e);
			return null;
		}
	}

	public void UpdateText()
	{
		if (entry != null)
		{
			MenuController.Instance.helpText.text = modSelector.Selected + " > " + configSelector.Selected + "\n-----------------------------------\n" + entry.Description.Description + $"\n\nDefault: {entry.DefaultValue}";
		}
	}
}
