using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("NGUI/NData/Slider Binding")]
public class NguiSliderBinding : NguiBinding
{
	private EZData.Property<float> _property;
	
	private float _prevValue = -1.0f;
	private bool _ignoreChanges = false;
	
	private UISlider _UiSliderReceiver;
	
	public float Min = 0;
	public float Max = 1;
	
	private float DataToSlider(float data)
	{
		if (Mathf.Abs(Max - Min) < float.Epsilon)
			return 0.0f;
		
		return (data - Min) / (Max - Min);
	}
	
	private float SliderToData(float slider)
	{
		if (Mathf.Abs(Max - Min) < float.Epsilon)
			return 0.0f;
		
		return Min + slider * (Max - Min);
	}
	
	public override void Awake()
	{
		base.Awake();
		
		_UiSliderReceiver = gameObject.GetComponent<UISlider>();
	}
	
	public override void UpdateBinding()
	{
		_property = null;
			
		var context = GetContext();
		if (context == null)
		{
			Debug.LogWarning("EZSliderBinding.UpdateBinding - context is null");
			return;
		}
		
		_property = context.FindProperty<float>(Path, this);
		
		if (_property != null)
		{
			_property.OnChange += OnChange;
			OnChange();
		}
	}
	
	void Update()
	{
		if (_UiSliderReceiver != null && _property != null)
		{
			if (_prevValue != _UiSliderReceiver.sliderValue)
			{
				_prevValue = _UiSliderReceiver.sliderValue;
				_ignoreChanges = true;
				_property.SetValue(SliderToData(_UiSliderReceiver.sliderValue));
				_ignoreChanges = false;
			}
		}
	}
	
	public void OnChange()
	{
		if (_ignoreChanges)
			return;
		
		if (_property == null || _UiSliderReceiver == null)
			return;
		
		_UiSliderReceiver.sliderValue = DataToSlider(_property.GetValue());
	}
}
