using UnityEngine;

namespace Grate.Extensions;

public static class TextureExtensions
{
	public static Texture2D ToTexture2D(this Texture texture)
	{
		return Texture2D.CreateExternalTexture(texture.width, texture.height, (TextureFormat)3, false, false, texture.GetNativeTexturePtr());
	}

	public static Texture2D? Copy(this Texture2D texture, TextureFormat? format = 5)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		if ((Object)(object)texture == (Object)null)
		{
			return null;
		}
		RenderTexture temporary = RenderTexture.GetTemporary(((Texture)texture).width, ((Texture)texture).height, 0, (RenderTextureFormat)7, (RenderTextureReadWrite)0);
		Graphics.Blit((Texture)(object)texture, temporary);
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = temporary;
		Texture2D val = new Texture2D(((Texture)texture).width, ((Texture)texture).height, format.HasValue ? format.Value : texture.format, 1 < ((Texture)texture).mipmapCount)
		{
			name = ((Object)texture).name
		};
		val.ReadPixels(new Rect(0f, 0f, (float)((Texture)temporary).width, (float)((Texture)temporary).height), 0, 0);
		val.Apply(true, false);
		RenderTexture.active = active;
		RenderTexture.ReleaseTemporary(temporary);
		return val;
	}
}
