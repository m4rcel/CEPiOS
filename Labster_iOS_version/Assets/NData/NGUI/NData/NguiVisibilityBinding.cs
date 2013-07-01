using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("NGUI/NData/Visibility Binding")]
public class NguiVisibilityBinding : NguiBooleanBinding
{
	private NguiVisibilityBinding _parentVisibility;
	private bool _selfVisible;
	
	private bool Visible
	{
		get 
		{
			if (_parentVisibility != null)
				return _selfVisible && _parentVisibility.Visible;
			else
				return _selfVisible;
		}	
	}
	
	public override void Awake()
	{
		base.Awake();
		_parentVisibility = NguiUtils.GetComponentInParents<NguiVisibilityBinding>(gameObject.transform.parent.gameObject);
	}
	
	protected override void ApplyNewValue(bool newValue)
	{
		if(this != null){
			_selfVisible = newValue;
			SetVisible(gameObject, Visible);
		}
	}
	
	private static void SetVisible(GameObject gameObject, bool visible)
	{
		SetNguiVisible(gameObject, visible);
		for (var i = 0; i < gameObject.transform.childCount; ++i)
		{
			var child = gameObject.transform.GetChild(i).gameObject;
			var childVisibilityBinding = child.GetComponent<NguiVisibilityBinding>();
			if (childVisibilityBinding != null)
			{
				SetVisible(child, visible && childVisibilityBinding._selfVisible);
			}
			else
			{
				SetVisible(child, visible);
			}
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
		foreach(var widget in gameObject.GetComponentsInChildren<UIWidget>())
		{
			widget.enabled = visible;
		}
		foreach(var panel in gameObject.GetComponentsInChildren<UIPanel>())
		{
			panel.enabled = visible;
		}
		
		
		foreach(var collider in gameObject.GetComponents<Collider>())
		{
			collider.enabled = visible;
		}
		foreach(var collider in gameObject.GetComponentsInChildren<Collider>())
		{
			collider.enabled = visible;
		}
	}
}
