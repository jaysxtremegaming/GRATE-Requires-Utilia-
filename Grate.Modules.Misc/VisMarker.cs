using System;
using System.Collections.Generic;
using Grate.Extensions;
using Grate.GUI;
using UnityEngine;

namespace Grate.Modules.Misc;

internal class VisMarker : MonoBehaviour
{
	private Transform anc;

	private VRRig rig;

	private GorillaSpeakerLoudness Speakerloudness;

	private List<Transform> VisParts;

	private void Start()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		rig = ((Component)this).GetComponent<VRRig>();
		anc = new GameObject("Vis").transform;
		VisParts = new List<Transform>();
		for (int i = 0; i < 50; i++)
		{
			GameObject val = GameObject.CreatePrimitive((PrimitiveType)0);
			((Component)(object)val.GetComponent<Collider>()).Obliterate();
			val.GetComponent<Renderer>().material = MenuController.Instance.grate[1];
			val.transform.SetParent(anc, false);
			val.transform.localScale = new Vector3(0.11612f, 0.11612f, 0.11612f);
			VisParts.Add(val.transform);
			Debug.Log((object)$"{i} shperes made");
		}
	}

	private void FixedUpdate()
	{
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)Speakerloudness == (Object)null)
		{
			Speakerloudness = ((Component)rig).GetComponent<GorillaSpeakerLoudness>();
		}
		if ((Object)(object)anc.parent == (Object)null)
		{
			anc.SetParent(((Component)rig).transform, false);
		}
		else if (VisParts.Count == 50)
		{
			int count = VisParts.Count;
			float num = 360f / (float)count;
			float smoothedLoudness = Speakerloudness.SmoothedLoudness;
			Vector3 position = ((Component)anc).transform.position;
			Vector3 position2 = default(Vector3);
			for (int i = 0; i < count; i++)
			{
				float num2 = (float)i * num;
				float num3 = smoothedLoudness * Mathf.Cos(num2 * (MathF.PI / 180f));
				float num4 = smoothedLoudness * Mathf.Sin(num2 * (MathF.PI / 180f));
				Vector3 val = position + new Vector3(num3, 0.2f, num4);
				float num5 = val.y + smoothedLoudness;
				((Vector3)(ref position2))._002Ector(val.x, num5, val.z);
				((Component)VisParts[i]).transform.position = position2;
				((Component)VisParts[i]).transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
			}
		}
	}

	private void OnDisable()
	{
		((Component)(object)anc).Obliterate();
	}

	private void OnDestory()
	{
		((Component)(object)anc).Obliterate();
	}
}
