using System.Collections.Generic;
using Grate;
using Grate.Extensions;
using UnityEngine;

public class ColliderRenderer : MonoBehaviour
{
	private Dictionary<Transform, BoxCollider> boxColliders;

	private Dictionary<Transform, MeshCollider> meshColliders;

	private Transform obj;

	private int refreshOffset;

	public float refreshRate = 10f;

	private Dictionary<Transform, SphereCollider> sphereColliders;

	private void Start()
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		refreshOffset = Random.Range(0, 60 * (int)refreshRate);
		boxColliders = new Dictionary<Transform, BoxCollider>();
		BoxCollider[] components = ((Component)this).GetComponents<BoxCollider>();
		foreach (BoxCollider val in components)
		{
			obj = GameObject.CreatePrimitive((PrimitiveType)3).transform;
			((Component)(object)((Component)obj).GetComponent<BoxCollider>()).Obliterate();
			Material val2 = Object.Instantiate<Material>(Plugin.AssetBundle.LoadAsset<GameObject>("Cloud").GetComponent<Renderer>().material);
			Color val3 = Random.ColorHSV();
			val3.a = 0.25f;
			val2.color = val3;
			val2.SetColor("_EmissionColor", val3);
			((Renderer)((Component)obj).GetComponent<MeshRenderer>()).material = val2;
			obj.SetParent(((Component)val).transform);
			boxColliders.Add(obj, val);
		}
		sphereColliders = new Dictionary<Transform, SphereCollider>();
		SphereCollider[] components2 = ((Component)this).GetComponents<SphereCollider>();
		foreach (SphereCollider val4 in components2)
		{
			obj = GameObject.CreatePrimitive((PrimitiveType)0).transform;
			((Component)(object)((Component)obj).GetComponent<SphereCollider>()).Obliterate();
			Material val5 = Object.Instantiate<Material>(Plugin.AssetBundle.LoadAsset<GameObject>("Cloud").GetComponent<Renderer>().material);
			Color val6 = Random.ColorHSV();
			val6.a = 0.25f;
			val5.color = val6;
			val5.SetColor("_EmissionColor", val6);
			((Renderer)((Component)obj).GetComponent<MeshRenderer>()).material = val5;
			obj.SetParent(((Component)val4).transform);
			sphereColliders.Add(obj, val4);
		}
		MeshCollider[] components3 = ((Component)this).GetComponents<MeshCollider>();
		foreach (MeshCollider val7 in components3)
		{
			obj = GameObject.CreatePrimitive((PrimitiveType)3).transform;
			((Component)(object)((Component)obj).GetComponent<BoxCollider>()).Obliterate();
			Material val8 = Object.Instantiate<Material>(Plugin.AssetBundle.LoadAsset<GameObject>("Cloud").GetComponent<Renderer>().material);
			Color val9 = Random.ColorHSV();
			val9.a = 0.25f;
			val8.color = val9;
			val8.SetColor("_EmissionColor", val9);
			((Renderer)((Component)obj).GetComponent<MeshRenderer>()).material = val8;
			obj.SetParent(((Component)val7).transform);
			meshColliders.Add(obj, val7);
		}
		Recalculate();
	}

	private void FixedUpdate()
	{
		if ((float)(refreshOffset + Time.frameCount) % (60f * refreshRate) == 0f)
		{
			Recalculate();
		}
	}

	private void OnDestroy()
	{
		((Component)(object)obj)?.Obliterate();
	}

	public void Recalculate()
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		foreach (KeyValuePair<Transform, BoxCollider> boxCollider in boxColliders)
		{
			Transform key = boxCollider.Key;
			BoxCollider value = boxCollider.Value;
			if (Object.op_Implicit((Object)(object)value))
			{
				key.localPosition = value.center;
				key.localScale = new Vector3(value.size.x, value.size.y, value.size.z);
			}
		}
		foreach (KeyValuePair<Transform, MeshCollider> meshCollider in meshColliders)
		{
			Transform key2 = meshCollider.Key;
			MeshCollider value2 = meshCollider.Value;
			if (Object.op_Implicit((Object)(object)value2))
			{
				Bounds bounds = ((Collider)value2).bounds;
				key2.localPosition = ((Bounds)(ref bounds)).center;
				bounds = ((Collider)value2).bounds;
				float x = ((Bounds)(ref bounds)).extents.x;
				bounds = ((Collider)value2).bounds;
				float y = ((Bounds)(ref bounds)).extents.y;
				bounds = ((Collider)value2).bounds;
				key2.localScale = new Vector3(x, y, ((Bounds)(ref bounds)).extents.z);
			}
		}
		foreach (KeyValuePair<Transform, SphereCollider> sphereCollider in sphereColliders)
		{
			Transform key3 = sphereCollider.Key;
			SphereCollider value3 = sphereCollider.Value;
			if (Object.op_Implicit((Object)(object)value3))
			{
				key3.localPosition = value3.center;
				key3.localScale = new Vector3(value3.radius * 2f, value3.radius * 2f, value3.radius * 2f);
			}
		}
	}
}
