using Grate.Tools;
using UnityEngine;

namespace Grate.Extensions;

public static class GameObjectExtensions
{
	public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
	{
		T component = obj.GetComponent<T>();
		if (!Object.op_Implicit((Object)(object)component))
		{
			return obj.AddComponent<T>();
		}
		return component;
	}

	public static void Log(this GameObject self)
	{
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		Logging.Debug("\"" + ((Object)self).name + "\": {");
		Logging.Debug("\"Components\": [");
		Component[] components = self.GetComponents<Component>();
		int num = 0;
		Component[] components2 = self.GetComponents<Component>();
		foreach (Component val in components2)
		{
			Logging.Debug($"\"{((object)val).GetType()}\"");
			num++;
			if (num < components.Length)
			{
				Logging.Debug(",");
			}
		}
		Logging.Debug("],");
		Logging.Debug("\"Children\": {");
		num = 0;
		foreach (Transform item in self.transform)
		{
			((Component)item).gameObject.Log();
			num++;
			if (num < self.transform.childCount)
			{
				Logging.Debug(",");
			}
		}
		Logging.Debug("}");
		Logging.Debug("}");
	}

	public static void Obliterate(this GameObject self)
	{
		Object.Destroy((Object)(object)self);
	}

	public static void Obliterate(this Component self)
	{
		Object.Destroy((Object)(object)self);
	}
}
