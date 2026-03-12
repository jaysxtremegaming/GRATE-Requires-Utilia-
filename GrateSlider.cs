using System;
using Grate.Tools;
using UnityEngine;
using UnityEngine.UI;

public class GrateSlider : MonoBehaviour
{
	private Knob _knob;

	private object _selected;

	private Transform knob;

	private Text label;

	public Action<object> OnValueChanged;

	private int selectedValue;

	private Transform sliderEnd;

	private Transform? sliderStart;

	private object[] values;

	public object Selected
	{
		get
		{
			return _selected;
		}
		set
		{
			_selected = value;
			OnValueChanged?.Invoke(value);
		}
	}

	private void Awake()
	{
		try
		{
			sliderStart = ((Component)this).transform.Find("Start");
			sliderEnd = ((Component)this).transform.Find("End");
			knob = ((Component)this).transform.Find("Knob");
			label = ((Component)this).GetComponentInChildren<Text>();
			_knob = ((Component)knob).gameObject.AddComponent<Knob>();
			_knob.Initialize(sliderStart, sliderEnd);
			Knob obj = _knob;
			obj.OnValueChanged = (Action<int>)Delegate.Combine(obj.OnValueChanged, (Action<int>)delegate(int value)
			{
				selectedValue = value;
				Selected = values[selectedValue];
				label.text = Selected.ToString();
			});
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	public void InitializeValues(object[] values, int initialValue)
	{
		try
		{
			this.values = values;
			selectedValue = initialValue;
			Selected = values[initialValue];
			label.text = Selected.ToString();
			_knob.divisions = values.Length - 1;
			_knob.Value = initialValue;
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}
}
