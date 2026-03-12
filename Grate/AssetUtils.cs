using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Grate;

public static class AssetUtils
{
	private static string FormatPath(string path)
	{
		return path.Replace("/", ".").Replace("\\", ".");
	}

	public static byte[] ExtractEmbeddedResource(string filePath)
	{
		filePath = FormatPath(filePath);
		using Stream stream = Assembly.GetCallingAssembly().GetManifestResourceStream(filePath);
		if (stream == null)
		{
			return null;
		}
		byte[] array = new byte[stream.Length];
		stream.Read(array, 0, array.Length);
		return array;
	}

	public static Texture2D GetTextureFromResource(string resourceName)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		byte[] array = ExtractEmbeddedResource(resourceName);
		if (array == null)
		{
			Console.WriteLine("No bytes found in " + resourceName);
			return null;
		}
		Texture2D val = new Texture2D(1, 1, (TextureFormat)4, false);
		ImageConversion.LoadImage(val, array);
		string text = resourceName.Substring(0, resourceName.LastIndexOf('.'));
		if (text.LastIndexOf('.') >= 0)
		{
			text = text.Substring(text.LastIndexOf('.') + 1);
		}
		((Object)val).name = text;
		return val;
	}

	public static AssetBundle? LoadAssetBundle(string path)
	{
		path = FormatPath(path);
		Stream? manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
		AssetBundle result = AssetBundle.LoadFromStream(manifestResourceStream);
		manifestResourceStream.Close();
		return result;
	}

	public static string[] GetResourceNames()
	{
		string[] manifestResourceNames = Assembly.GetCallingAssembly().GetManifestResourceNames();
		if (manifestResourceNames == null)
		{
			Console.WriteLine("No manifest resources found.");
			return null;
		}
		return manifestResourceNames;
	}

	public static Sprite LoadSprite(string path)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		Texture2D textureFromResource = GetTextureFromResource(path);
		return Sprite.Create(textureFromResource, new Rect(0f, 0f, (float)((Texture)textureFromResource).width, (float)((Texture)textureFromResource).height), Vector2.zero);
	}
}
