using System;
using System.Collections.Generic;
using Grate.Extensions;
using UnityEngine;

namespace Grate.Tools;

public class DebugRay : MonoBehaviour
{
	public static Dictionary<string, DebugRay> rays = new Dictionary<string, DebugRay>();

	public Color color = Color.red;

	public LineRenderer lineRenderer;

	public void Awake()
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		Logging.Debug(((Object)this).name);
		lineRenderer = ((Component)this).gameObject.GetOrAddComponent<LineRenderer>();
		lineRenderer.startColor = color;
		lineRenderer.startWidth = 0.01f;
		lineRenderer.endWidth = 0.01f;
		((Renderer)lineRenderer).material = Plugin.AssetBundle.LoadAsset<Material>("X-Ray Material");
	}

	public void Set(Vector3 start, Vector3 direction)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			((Renderer)lineRenderer).material.color = color;
			lineRenderer.SetPosition(0, start);
			lineRenderer.SetPosition(1, start + direction);
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	public void Set(Ray ray)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		Set(((Ray)(ref ray)).origin, ((Ray)(ref ray)).direction);
	}

	public static DebugRay Get(string name)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		if (rays.ContainsKey(name))
		{
			return rays[name];
		}
		DebugRay debugRay = new GameObject(name + " (Debug Ray)").AddComponent<DebugRay>();
		rays.Add(name, debugRay);
		return debugRay;
	}

	public DebugRay SetColor(Color c)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		color = c;
		return this;
	}
}
