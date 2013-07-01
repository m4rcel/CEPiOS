using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("NGUI/NData/Comparison Invisibility Binding")]
public class ComparisonInvisibilityBinding : NguiBinding
{
	
	public string Comparer;
	
	private readonly Dictionary<Type, EZData.Property> _properties = new Dictionary<Type, EZData.Property>();
	
	private bool _ignoreChanges = false;
		
	[HideInInspector]
	public delegate void OnValueChangeDelegate(string newValue);
	
	//[HideInInspector]
	//public event OnValueChangeDelegate OnValueChange;
	
	public override void Awake()
	{
		base.Awake();
		
		_properties.Add(typeof(bool), null);
		_properties.Add(typeof(string), null);
		_properties.Add(typeof(int), null);
		_properties.Add(typeof(float), null);
		_properties.Add(typeof(double), null);
		_properties.Add(typeof(decimal), null);
		_properties.Add(typeof(string[]), null);
		_properties.Add(typeof(DateTime), null);
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
			Debug.LogWarning("EZTextBinding.UpdateBinding - context is null");
			return;
		}
		
		_properties[typeof(bool)] = context.FindProperty<bool>(Path, this);
		_properties[typeof(string)] = context.FindProperty<string>(Path, this);
		_properties[typeof(int)] = context.FindProperty<int>(Path, this);
		_properties[typeof(float)] = context.FindProperty<float>(Path, this);
		_properties[typeof(double)] = context.FindProperty<double>(Path, this);
		_properties[typeof(decimal)] = context.FindProperty<decimal>(Path, this);
		_properties[typeof(DateTime)] = context.FindProperty<DateTime>(Path, this);
		_properties[typeof(string[])] = context.FindProperty<string[]>(Path, this);
		
		foreach(var p in _properties)
		{
			if (p.Value != null)
			{
				p.Value.OnChange += OnChange;
				OnChange();
			}
		}
	}
	
	void Update()
	{
		//OnChange();
//		if (_UiInputReceiver != null)
//		{
//			if (_UiInputReceiver.text != _prevFrameInputText)
//			{
//				_prevFrameInputText = _UiInputReceiver.text;
//				_ignoreChanges = true;
//				SetValue(_UiInputReceiver.text);
//				_ignoreChanges = false;
//			}
//		}
	}
	
	public void OnChange(){
		
		if (this==null){
			foreach(var p in _properties)
			{
				if (p.Value != null)
				{
					p.Value.OnChange -= OnChange;
				}
			}
			return;
		}
		
		if (_ignoreChanges)
			return;
		
		
		if (_properties[typeof(string)] != null){
			var newValue = ((EZData.Property<string>)_properties[typeof(string)]).GetValue();
			SetNguiVisible(gameObject, Comparer!=newValue);
		}
		if (_properties[typeof(bool)] != null){
			
			var newValue = ((EZData.Property<bool>)_properties[typeof(bool)]).GetValue();
			bool v=false;
			if (bool.TryParse(Comparer, out v))
				SetNguiVisible(gameObject, v!=newValue);
		}
		if (_properties[typeof(int)] != null){
			var newValue = ((EZData.Property<int>)_properties[typeof(int)]).GetValue();
			int v = 0;
			if (int.TryParse(Comparer, out v))
				SetNguiVisible(gameObject, v!=newValue);
		}
		if (_properties[typeof(float)] != null){
			var newValue = ((EZData.Property<float>)_properties[typeof(float)]).GetValue();
			float v = 0;
			if (float.TryParse(Comparer, out v))
				SetNguiVisible(gameObject, v!=newValue);
		}
		if (_properties[typeof(double)] != null){
			var newValue = ((EZData.Property<double>)_properties[typeof(double)]).GetValue();
			double v = 0;
			if (double.TryParse(Comparer, out v))
				SetNguiVisible(gameObject, v!=newValue);
		}
		if (_properties[typeof(decimal)] != null){
			var newValue = ((EZData.Property<decimal>)_properties[typeof(decimal)]).GetValue();
			decimal v = 0;
			if (decimal.TryParse(Comparer, out v))
				SetNguiVisible(gameObject, v!=newValue);
		}
		if (_properties[typeof(string[])] != null){
			string[] newValue = ((EZData.Property<string[]>)_properties[typeof(string[])]).GetValue();
			var hasMatch = !newValue.Contains(Comparer);
			SetNguiVisible(gameObject, hasMatch);
		}
	}
	
	private static void SetNguiVisible(GameObject gameObject, bool visible)
	{
		foreach(var widget in gameObject.GetComponents<UIWidget>())
		{
			widget.enabled = visible;
		}		
		foreach(var panel in gameObject.GetComponents<UIPanel>())
		{
			panel.enabled = visible;
		}
		
		foreach(var widget in gameObject.transform.GetComponentsInChildren<UIWidget>(true))
		{
			widget.enabled = visible;
		}
		foreach(var panel in gameObject.transform.GetComponentsInChildren<UIPanel>(true))
		{
			panel.enabled = visible;
		}
		
		foreach(var collider in gameObject.GetComponents<Collider>())
		{
			collider.enabled = visible;
		}
		foreach(var collider in gameObject.transform.GetComponentsInChildren<Collider>(true))
		{
			collider.enabled = visible;
		}
	}
}
