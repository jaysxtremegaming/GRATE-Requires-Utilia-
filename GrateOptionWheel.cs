using System;
using System.Collections.Generic;
using Grate.Extensions;
using Grate.Tools;
using UnityEngine;
using UnityEngine.UI;

public class GrateOptionWheel : MonoBehaviour
{
	private string _selected;

	private Transform cylinder;

	private Text[] labels;

	public Action<string> OnValueChanged;

	private int selectedValue;

	private int selectedLabel;

	private ButtonController upButton;

	private ButtonController downButton;

	private List<string> values;

	public string Selected
	{
		get
		{
			return _selected;
		}
		private set
		{
			_selected = value;
			OnValueChanged?.Invoke(value);
		}
	}

	private void Awake()
	{
		try
		{
			cylinder = ((Component)this).transform.Find("Cylinder");
			labels = ((Component)cylinder).GetComponentsInChildren<Text>();
			upButton = ((Component)((Component)this).transform.Find("Arrow Up")).gameObject.AddComponent<ButtonController>();
			downButton = ((Component)((Component)this).transform.Find("Arrow Down")).gameObject.AddComponent<ButtonController>();
			upButton.buttonPushDistance = 0.01f;
			downButton.buttonPushDistance = 0.01f;
			ButtonController buttonController = upButton;
			buttonController.OnPressed = (Action<ButtonController, bool>)Delegate.Combine(buttonController.OnPressed, (Action<ButtonController, bool>)delegate(ButtonController button, bool pressed)
			{
				Cycle(-1);
				button.IsPressed = false;
			});
			ButtonController buttonController2 = downButton;
			buttonController2.OnPressed = (Action<ButtonController, bool>)Delegate.Combine(buttonController2.OnPressed, (Action<ButtonController, bool>)delegate(ButtonController button, bool pressed)
			{
				Cycle(1);
				button.IsPressed = false;
			});
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	private void FixedUpdate()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			float num = (float)(selectedLabel % 6) * 60f;
			cylinder.localRotation = Quaternion.Slerp(cylinder.localRotation, Quaternion.Euler(num, 0f, 0f), Time.fixedDeltaTime * 10f);
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	private void Cycle(int direction)
	{
		try
		{
			selectedValue = MathExtensions.Wrap(selectedValue + direction, 0, values.Count);
			selectedLabel = MathExtensions.Wrap(selectedLabel + direction, 0, labels.Length);
			Selected = values[selectedValue];
			int num = MathExtensions.Wrap(selectedLabel + 2 * direction, 0, labels.Length);
			string text = values[MathExtensions.Wrap(selectedValue + 2 * direction, 0, values.Count)];
			labels[num].text = text;
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	public void InitializeValues(List<string> values)
	{
		try
		{
			selectedLabel = 0;
			selectedValue = 0;
			this.values = values;
			Selected = values[selectedValue];
			for (int i = 0; i < labels.Length; i++)
			{
				int index = ((i >= labels.Length / 2) ? MathExtensions.Wrap(values.Count - labels.Length + i, 0, values.Count) : MathExtensions.Wrap(selectedValue + i, 0, values.Count));
				int num = MathExtensions.Wrap(selectedLabel + i, 0, labels.Length);
				labels[num].text = values[index];
			}
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}
}
