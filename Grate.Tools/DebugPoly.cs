using System;
using UnityEngine;

namespace Grate.Tools;

public class DebugPoly : MonoBehaviour
{
	public Mesh mesh;

	public Renderer renderer;

	public Vector3[] vertices = (Vector3[])(object)new Vector3[0];

	private void Awake()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			mesh = new Mesh();
			GameObject val = new GameObject("Debug Polygon");
			val.transform.parent = ((Component)this).transform;
			renderer = (Renderer)(object)val.AddComponent<MeshRenderer>();
			renderer.material = Plugin.AssetBundle.LoadAsset<Material>("Cloud Material");
			renderer.material.color = new Color(1f, 1f, 1f, 0.1f);
			val.AddComponent<MeshFilter>().mesh = mesh;
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	private void FixedUpdate()
	{
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (vertices.Length == 3)
			{
				SetColor((float)Time.frameCount / 1000f % 1f, 1f, 1f);
				mesh.Clear();
				mesh.vertices = vertices;
				mesh.uv = (Vector2[])(object)new Vector2[3]
				{
					new Vector2(vertices[0].x, vertices[0].y),
					new Vector2(vertices[1].x, vertices[1].y),
					new Vector2(vertices[2].x, vertices[2].y)
				};
				mesh.triangles = new int[3] { 2, 1, 0 };
				mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 2000f);
			}
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	public void SetColor(float h, float s, float v, float a = 0.1f)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		Color val = Color.HSVToRGB(h, s, v);
		val.a = a;
		renderer.material.color = val;
		renderer.material.SetColor("_EmissionColor", val);
	}
}
