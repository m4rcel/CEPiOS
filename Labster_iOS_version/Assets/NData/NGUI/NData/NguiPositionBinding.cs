using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("NGUI/NData/Text Binding")]
public class NguiPositionBinding : NguiBinding
{	
	private readonly Dictionary<Type, EZData.Property> _properties = new Dictionary<Type, EZData.Property>();
	
	private bool _ignoreChanges = false;
	
	public Transform TargetTransform;
	
	//private UILabel _UiLabelReceiver;
		
	[HideInInspector]
	public delegate void OnValueChangeDelegate(string newValue);
	
	//[HideInInspector]
	//public event OnValueChangeDelegate OnValueChange;
	
	public override void Awake()
	{
		base.Awake();		
		_properties.Add(typeof(Vector3), null);
	}
	
	public override void Start()
	{
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
			Debug.LogWarning("EZTransformBinding.UpdateBinding - context is null");
			return;
		}
		
		_properties[typeof(Vector3)] = context.FindProperty<Vector3>(Path, this);		
		
		
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

	}
	
	public void OnChange()
	{
		if (_ignoreChanges)
			return;
		
		var newValue = new Vector3();
		newValue = ((EZData.Property<Vector3>)_properties[typeof(Vector3)]).GetValue();
		
		transform.position = newValue;	
	}
}
