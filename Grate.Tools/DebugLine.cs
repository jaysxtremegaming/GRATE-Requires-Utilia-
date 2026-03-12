using System;
using UnityEngine;

namespace Grate.Tools;

public class DebugLine : MonoBehaviour
{
	public Transform a;

	public Transform b;

	public LineRenderer lineRenderer;

	private void Start()
	{
		lineRenderer = ((Component)this).gameObject.AddComponent<LineRenderer>();
		lineRenderer.startWidth = 0.1f;
		lineRenderer.endWidth = 0.1f;
	}

	private void FixedUpdate()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		if (Object.op_Implicit((Object)(object)a) && Object.op_Implicit((Object)(object)b))
		{
			Connect(a.position, b.position);
		}
	}

	public void Connect(Transform a, Transform b)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)a == (Object)null || (Object)(object)b == (Object)null)
		{
			throw new NullReferenceException("Transform(s) null: " + ((a != null) ? ((Object)a).name : null) + ", " + ((b != null) ? ((Object)b).name : null));
		}
		this.a = a;
		this.b = b;
		Connect(a.position, b.position);
	}

	public void Connect(Vector3 a, Vector3 b)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		lineRenderer.SetPositions((Vector3[])(object)new Vector3[2] { a, b });
	}
}
