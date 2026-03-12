using System;
using System.Collections.Generic;
using System.IO;
using Grate.Extensions;
using UnityEngine;

namespace Grate.Tools;

public static class TextureRipper
{
	public static string folderName = "C:\\Users\\ultra\\Pictures\\Gorilla Tag Textures";

	public static void Rip()
	{
		string text = "Start";
		Directory.CreateDirectory(folderName);
		try
		{
			text = "Locating renderers";
			Renderer[] array = Object.FindObjectsOfType<Renderer>();
			Logging.Debug("Found", array.Length, "renderers");
			text = "Looping through renderers";
			List<Texture> list = new List<Texture>();
			Renderer[] array2 = array;
			foreach (Renderer val in array2)
			{
				text = "Formatting file path";
				text = "Storing materials";
				Material[] sharedMaterials = val.sharedMaterials;
				for (int j = 0; j < sharedMaterials.Length; j++)
				{
					try
					{
						text = "Getting main texutre";
						Texture mainTexture = sharedMaterials[j].mainTexture;
						if ((Object)(object)mainTexture != (Object)null && !list.Contains(mainTexture))
						{
							list.Add(mainTexture);
							text = "Creating directory";
							text = "Encoding to png";
							byte[] array3 = ImageConversion.EncodeToPNG(((Texture2D)(object)((mainTexture is Texture2D) ? mainTexture : null)).Copy((TextureFormat)5));
							text = "Getting material name";
							string name = ((Object)sharedMaterials[j]).name;
							text = "Getting file name";
							string text2 = Path.Combine(folderName, ((Object)((Component)val).gameObject).name + "--" + name + ".png");
							text = "Writing bytes";
							if (!text2.Contains("plastickey"))
							{
								Logging.Debug(text2, array3);
								File.WriteAllBytes(text2, array3);
							}
						}
					}
					catch (Exception e)
					{
						Logging.Exception(e);
					}
				}
			}
		}
		catch (Exception e2)
		{
			Logging.Warning("Failed at step", text);
			Logging.Exception(e2);
		}
	}
}
