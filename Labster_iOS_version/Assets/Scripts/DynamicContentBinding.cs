using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class DynamicContentBinding : NguiBinding
{	
	public string 	Property;
	public Vector3 lowestPoint;
	
	private Vector3 _initialPosition;
	
	private readonly Dictionary<Type, EZData.Property> _properties = new Dictionary<Type, EZData.Property>();
	
	//private bool _ignoreChanges = false;
	private bool _init;
	public override void Awake()
	{
		base.Awake();
		
		//_properties.Add(typeof(float), null);
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
			Debug.LogWarning("EZTextBinding.UpdateBinding - context is null");
			return;
		}
		
		//_properties[typeof(float)] = context.FindProperty<float>(Property, null);
		
//		foreach(var p in _properties)
//		{
//			if (p.Value != null)
//			{
//				p.Value.OnChange += OnChange;
//				OnChange();
//			}
//		}
	}
	
	public void UpdateContent(float newValue, Color newColor, Transform contentObject, Transform lowerContent){
		
		if(!_init){
			_initialPosition = transform.localPosition;
			_init=true;
		}
				
		
		if(this!=null){			
			
			// TEMPORARY!!! (until positioning is updated)
			if (newValue > 0){
				transform.localPosition = new Vector3(_initialPosition.x, _initialPosition.y, _initialPosition.z * newValue);
			}else{
				transform.localPosition = new Vector3(_initialPosition.x, _initialPosition.y, 0);
			}
			// TEMPORARY END
			
			/*
			transform.localPosition = new Vector3(
				_initialPosition.x,
				_initialPosition.y,
				(_initialPosition.z*newValue)+lowestPoint.z
			);
			
			transform.localScale = new Vector3(
				1f-(0.18f*(1-newValue)),
				1f-(0.18f*(1-newValue)),
				1
			);
			*/
			
			if(contentObject){
				contentObject.renderer.enabled = (newValue>0.05f);
				contentObject.renderer.material.color = newColor;
				contentObject.renderer.material.SetColor("_Emission", newColor);
			}
			if(lowerContent){
				lowerContent.renderer.enabled = (newValue>0.05f);
				lowerContent.renderer.material.color = newColor;
				lowerContent.renderer.material.SetColor("_Emission", newColor);
			}
			
		}
	}
}
