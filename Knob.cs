using System;
using Grate.GUI;
using Grate.Gestures;
using Grate.Interaction;
using Grate.Tools;
using UnityEngine;

public class Knob : GrateInteractable
{
	public int divisions;

	public Action<int>? OnValueChanged;

	private Transform? start;

	private Transform? end;

	private int value;

	public int Value
	{
		get
		{
			return value;
		}
		set
		{
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			if (value != this.value)
			{
				OnValueChanged?.Invoke(value);
				if (base.Selected)
				{
					GestureTracker.Instance.HapticPulse(selectors[0].IsLeft);
				}
				Sounds.Play(Sounds.Sound.keyboardclick);
			}
			this.value = value;
			((Component)this).transform.position = Vector3.Lerp(start.position, end.position, (float)Value / (float)divisions);
		}
	}

	private void FixedUpdate()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		if (base.Selected)
		{
			Vector3 val = ((Component)selectors[0]).transform.position - start.position;
			Vector3 val2 = end.position - start.position;
			float num = Vector3.Dot(val2, val) / ((Vector3)(ref val2)).magnitude;
			num = Mathf.Clamp01(num / ((Vector3)(ref val2)).magnitude);
			Value = Mathf.RoundToInt(num * (float)divisions);
		}
	}

	public void Initialize(Transform? start, Transform end)
	{
		priority = MenuController.Instance.priority;
		this.start = start;
		this.end = end;
	}
}
