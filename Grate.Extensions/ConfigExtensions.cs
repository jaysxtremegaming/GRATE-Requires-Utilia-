using System;
using BepInEx.Configuration;
using UnityEngine;

namespace Grate.Extensions;

public static class ConfigExtensions
{
	public struct ConfigValueInfo
	{
		public object[] AcceptableValues;

		public int InitialValue;
	}

	public static ConfigValueInfo ValuesInfo(this ConfigEntryBase entry)
	{
		if (entry.SettingType == typeof(bool))
		{
			ConfigValueInfo result = default(ConfigValueInfo);
			result.AcceptableValues = new object[2] { false, true };
			result.InitialValue = (((bool)entry.BoxedValue) ? 1 : 0);
			return result;
		}
		if (entry.SettingType == typeof(int))
		{
			ConfigValueInfo result = default(ConfigValueInfo);
			result.AcceptableValues = new object[11]
			{
				0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
				10
			};
			result.InitialValue = Mathf.Clamp((int)entry.BoxedValue, 0, 10);
			return result;
		}
		if (entry.SettingType == typeof(string))
		{
			string[] acceptableValues = ((AcceptableValueList<string>)(object)entry.Description.AcceptableValues).AcceptableValues;
			for (int i = 0; i < acceptableValues.Length; i++)
			{
				if (acceptableValues[i] == (string)entry.BoxedValue)
				{
					ConfigValueInfo result = default(ConfigValueInfo);
					object[] acceptableValues2 = acceptableValues;
					result.AcceptableValues = acceptableValues2;
					result.InitialValue = i;
					return result;
				}
			}
		}
		throw new Exception($"Unknown config type {entry.SettingType}");
	}
}
