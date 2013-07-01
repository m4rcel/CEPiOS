using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("NGUI/NData/Text Binding")]
public class NguiImageBinding : NguiBinding
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
	
	private UISprite _UiInputReceiver;
	//private UILabel _UiLabelReceiver;
		
	[HideInInspector]
	public delegate void OnValueChangeDelegate(string newValue);
	
	//[HideInInspector]
	//public event OnValueChangeDelegate OnValueChange;
	
	public override void Awake()
	{
		base.Awake();		
		_properties.Add(typeof(string), null);
		
		_UiInputReceiver = gameObject.GetComponent<UISprite>();
	}
	
	public override void Start()
	{
		base.Start();
	}
	
	public override void UpdateBinding()
	{
		var context = GetContext();
		if (context == null)
		{
			Debug.LogWarning("EZImageBinding.UpdateBinding - context is null");
			return;
		}
		if (Path!=null && _properties[typeof(string)] != context.FindProperty<string>(Path, this) ){
			
			foreach(var p in _properties)
			{
				if (p.Value != null)
				{
					p.Value.OnChange -= OnChange;
					_properties[p.Key] = null;
					break;
				}
			}
			
			_properties[typeof(string)] = context.FindProperty<string>(Path, this);		
			
			foreach(var p in _properties)
			{
				if (p.Value != null)
				{
					p.Value.OnChange += OnChange;
					OnChange();
				}
			}
			
		}
		else if (_properties[typeof(string)] == null){
			OnChange();
		}
	}
	
//	public void SetValue(string newValue)
//	{
//		if (_properties[typeof(string)] != null)
//		{
//			((EZData.Property<string>)_properties[typeof(string)]).SetValue(newValue);
//		}
//	}
	void Update()
	{		
		if (_UiInputReceiver != null)
		{
			if (_UiInputReceiver.spriteName != _prevFrameInputText)
			{
				_prevFrameInputText = _UiInputReceiver.spriteName;
				_ignoreChanges = true;
				_ignoreChanges = false;
			}
		}
		
		UpdateBinding();
	}
	
	public void OnChange()
	{
		if (_ignoreChanges)
			return;
		
		var newValue = "";
		if (_properties.ContainsKey(typeof(string)) && _properties[typeof(string)] != null)
			newValue = string.Format(LocalizedFormat, ((EZData.Property<string>)_properties[typeof(string)]).GetValue());
		
		if(newValue.Length == 0)
		{
			Destroy(gameObject.GetComponent<UISprite>());
		}
		else
		{
			gameObject.GetComponent<UISprite>().spriteName = newValue;
			gameObject.GetComponent<UISprite>().MakePixelPerfect();
		}
	}
}
