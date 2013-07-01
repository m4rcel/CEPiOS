using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("NGUI/NData/Number to Time Binding")]
public class NumberToTimeBinding : NguiBinding
{
	public string Format = "{0}";
	
	virtual protected string LocalizedFormat
	{ 
		get 
		{
			var rootContext = GetContext().Root;
			if (rootContext == null)
			{
				return Format;
			}
			
			return Format; 
		} 
	} 
	
	private readonly Dictionary<Type, EZData.Property> _properties = new Dictionary<Type, EZData.Property>();
	
	private bool _ignoreChanges = false;
	private string _prevFrameInputText = "";
	
	private UIInput _UiInputReceiver;
	private UILabel _UiLabelReceiver;
		
	[HideInInspector]
	public delegate void OnValueChangeDelegate(string newValue);
	
	[HideInInspector]
	public event OnValueChangeDelegate OnValueChange;
	
	public override void Awake()
	{
		base.Awake();
		
		_properties.Add(typeof(string), null);
		_properties.Add(typeof(int), null);
		_properties.Add(typeof(float), null);
		_properties.Add(typeof(double), null);
		_properties.Add(typeof(decimal), null);
		_properties.Add(typeof(DateTime), null);
		
		_UiInputReceiver = gameObject.GetComponent<UIInput>();
		
		_UiLabelReceiver = gameObject.GetComponent<UILabel>();
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
		
		_properties[typeof(string)] 	= context.FindProperty<string>(Path, this);
		_properties[typeof(int)] 		= context.FindProperty<int>(Path, this);
		_properties[typeof(float)] 		= context.FindProperty<float>(Path, this);
		_properties[typeof(double)] 	= context.FindProperty<double>(Path, this);
		_properties[typeof(decimal)] 	= context.FindProperty<decimal>(Path, this);
		_properties[typeof(DateTime)] 	= context.FindProperty<DateTime>(Path, this);
		
		foreach(var p in _properties)
		{
			if (p.Value != null)
			{
				p.Value.OnChange += OnChange;
				OnChange();
			}
		}
	}
	
	public void SetValue(string newValue)
	{
		if (_properties[typeof(string)] != null)
		{
			((EZData.Property<string>)_properties[typeof(string)]).SetValue(newValue);
		}
		
		if (_properties[typeof(int)] != null)
		{
			int v = 0;
			if (int.TryParse(newValue, out v))
				((EZData.Property<int>)_properties[typeof(int)]).SetValue(v);
		}
		
		if (_properties[typeof(float)] != null)
		{
			float v = 0;
			if (float.TryParse(newValue, out v))
				((EZData.Property<float>)_properties[typeof(float)]).SetValue(v);
		}
		
		if (_properties[typeof(double)] != null)
		{
			double v = 0;
			if (double.TryParse(newValue, out v))
				((EZData.Property<double>)_properties[typeof(double)]).SetValue(v);
		}

		if (_properties[typeof(decimal)] != null)
		{
			decimal v = 0;
			if (decimal.TryParse(newValue, out v))
				((EZData.Property<decimal>)_properties[typeof(decimal)]).SetValue(v);
		}		
	}
	
	void Update()
	{
		if (_UiInputReceiver != null)
		{
			if (_UiInputReceiver.text != _prevFrameInputText)
			{
				_prevFrameInputText = _UiInputReceiver.text;
				_ignoreChanges = true;
				SetValue(_UiInputReceiver.text);
				_ignoreChanges = false;
			}
		}
		
		UpdateBinding();
	}
	
	//parse gametimer to time, 100gametimer = 1sec
	string ParseTime(int time){
		time=(int)Mathf.Floor(time/100);
		string seconds = "0"+(time%60).ToString();
		string minutes = "0"+(Mathf.Floor((time/60f)%60).ToString());
		string hours = Mathf.Floor(time/3600f).ToString();
		
		return hours+":"+minutes[minutes.Length-2]+minutes[minutes.Length-1]+   ":"   +seconds[seconds.Length-2]+""+seconds[seconds.Length-1];
	}
	
	public void OnChange()
	{
		if (_ignoreChanges)
			return;
		
		var newValue = "";
		
		if (_properties[typeof(string)] != null)
			newValue = string.Format(LocalizedFormat, ((EZData.Property<string>)_properties[typeof(string)]).GetValue());
		if (_properties[typeof(int)] != null)
			newValue = string.Format(LocalizedFormat, ((EZData.Property<int>)_properties[typeof(int)]).GetValue());
		if (_properties[typeof(float)] != null)
			newValue = string.Format(LocalizedFormat, ((EZData.Property<float>)_properties[typeof(float)]).GetValue());
		if (_properties[typeof(double)] != null)
			newValue = string.Format(LocalizedFormat, ((EZData.Property<double>)_properties[typeof(double)]).GetValue());
		if (_properties[typeof(decimal)] != null)
			newValue = string.Format(LocalizedFormat, ((EZData.Property<decimal>)_properties[typeof(decimal)]).GetValue());
		if (_properties[typeof(DateTime)] != null)
			newValue = string.Format(LocalizedFormat, ((EZData.Property<DateTime>)_properties[typeof(DateTime)]).GetValue());
		
		
		newValue = ParseTime(int.Parse(newValue));
		
		if (OnValueChange != null)
		{
			OnValueChange(newValue);
		}
		
		if (_UiInputReceiver != null)
		{
			_UiInputReceiver.text = newValue;
		}
		
		if (_UiLabelReceiver != null)
		{
			_UiLabelReceiver.text = newValue;
		}
	}
}
