using System.Collections.Generic;
using UnityEngine;

public static class ColorExtensions
{
	private static readonly Dictionary<Color, string> ColorMap = new Dictionary<Color, string>
	{
		{
			Color.red,
			"Red"
		},
		{
			Color.green,
			"Green"
		},
		{
			Color.blue,
			"Blue"
		},
		{
			Color.yellow,
			"Yellow"
		},
		{
			Color.magenta,
			"Magenta"
		},
		{
			Color.cyan,
			"Cyan"
		},
		{
			Color.white,
			"White"
		},
		{
			Color.black,
			"Black"
		},
		{
			Color.gray,
			"Gray"
		},
		{
			Color.clear,
			"Clear"
		}
	};

	public static Color StringToColor(this string input)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		if (input.StartsWith("#"))
		{
			Color result = default(Color);
			ColorUtility.TryParseHtmlString(input, ref result);
			return result;
		}
		string text = input.ToLower();
		foreach (KeyValuePair<Color, string> item in ColorMap)
		{
			if (item.Value.ToLower() == text)
			{
				return item.Key;
			}
		}
		return Color.white;
	}

	public static string ColorName(this Color color)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		if (ColorMap.TryGetValue(color, out string value))
		{
			return value;
		}
		return "#" + ColorUtility.ToHtmlStringRGBA(color);
	}
}
