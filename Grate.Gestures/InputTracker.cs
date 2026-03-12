using System;
using HarmonyLib;
using UnityEngine;
using UnityEngine.XR;

namespace Grate.Gestures;

public abstract class InputTracker
{
	public string name;

	public XRNode node;

	public Action<InputTracker> OnPressed;

	public Action<InputTracker> OnReleased;

	public bool pressed;

	public bool wasPressed;

	public Quaternion quaternionValue;

	public Traverse traverse;

	public Vector3 vector3Value;

	public abstract void UpdateValues();
}
public class InputTracker<T> : InputTracker
{
	public InputTracker(Traverse traverse, XRNode node)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		base.traverse = traverse;
		base.node = node;
	}

	public T GetValue()
	{
		return traverse.GetValue<T>();
	}

	public override void UpdateValues()
	{
		wasPressed = pressed;
		if (typeof(T) == typeof(bool))
		{
			pressed = traverse.GetValue<bool>();
		}
		else if (typeof(T) == typeof(float))
		{
			pressed = traverse.GetValue<float>() > 0.5f;
		}
		if (!wasPressed && pressed)
		{
			OnPressed?.Invoke(this);
		}
		if (wasPressed && !pressed)
		{
			OnReleased?.Invoke(this);
		}
	}
}
