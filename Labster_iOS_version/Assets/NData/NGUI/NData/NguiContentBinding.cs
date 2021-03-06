using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("NGUI/NData/Text Binding")]
public class NguiContentBinding : NguiBinding
{
	private GameObject _contentObject;
	
	private Vector3 initialScale;
	private Transform progressBarBody;
	
	private readonly Dictionary<Type, EZData.Property> _properties = new Dictionary<Type, EZData.Property>();
	
	private bool _ignoreChanges = false;
	private string _prevFrameInputText = "";
	
	private UIInput _UiInputReceiver;
	//private UILabel _UiLabelReceiver;
		
	[HideInInspector]
	public delegate void OnValueChangeDelegate(string newValue);
	
	//[HideInInspector]
	//public event OnValueChangeDelegate OnValueChange;
	
	public override void Awake()
	{
		base.Awake();		
		_properties.Add(typeof(int), null);
		
		_UiInputReceiver = gameObject.GetComponent<UIInput>();
	}
	
	public override void Start()
	{
		progressBarBody 	= transform;
		
		initialScale		= progressBarBody.localScale;
		base.Start();
	}
	
	public override void UpdateBinding()
	{		
		foreach(var p in _properties)
		{
			if (p.Value != null)
			{
				p.Value.OnChange -= OnChange;
				_properties[p.Key] = null;
				break;
			}
		}
			
		var context = GetContext();
		if (context == null)
		{
			Debug.LogWarning("EZWidthBinding.UpdateBinding - context is null");
			return;
		}
		
		_properties[typeof(int)] = context.FindProperty<int>(Path, this);		
		
		
		foreach(var p in _properties)
		{
			if (p.Value != null)
			{
				p.Value.OnChange += OnChange;
				OnChange();
			}
		}
	}
	
//	public void SetValue(string newValue)
//	{
//		if (_properties[typeof(string)] != null)
//		{
//			((EZData.Property<string>)_properties[typeof(string)]).SetValue(newValue);
//		}
//		
//		if (_properties[typeof(int)] != null)
//		{			
//			int v = 0;
//			if (int.TryParse(newValue, out v))
//				((EZData.Property<int>)_properties[typeof(int)]).SetValue(v);
//		}
//		
//		if (_properties[typeof(float)] != null)
//		{
//			float v = 0;
//			if (float.TryParse(newValue, out v))
//				((EZData.Property<float>)_properties[typeof(float)]).SetValue(v);
//		}
//		
//		if (_properties[typeof(double)] != null)
//		{
//			double v = 0;
//			if (double.TryParse(newValue, out v))
//				((EZData.Property<double>)_properties[typeof(double)]).SetValue(v);
//		}
//
//		if (_properties[typeof(decimal)] != null)
//		{
//			decimal v = 0;
//			if (decimal.TryParse(newValue, out v))
//				((EZData.Property<decimal>)_properties[typeof(decimal)]).SetValue(v);
//		}		
//	}
	
	void Update()
	{		
		if (_UiInputReceiver != null)
		{
			if (_UiInputReceiver.text != _prevFrameInputText)
			{
				_prevFrameInputText = _UiInputReceiver.text;
				_ignoreChanges = true;
				_ignoreChanges = false;
			}
		}
	}
	
	public void OnChange()
	{
		if (_ignoreChanges)
			return;
		
		var newValue = 0;		
		newValue = ((EZData.Property<int>)_properties[typeof(int)]).GetValue();
		
		if(progressBarBody)
		progressBarBody.localScale = new Vector3(initialScale.x*(newValue/100f), initialScale.y, initialScale.z);
	
	}
}
