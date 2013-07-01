using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("NGUI/NData/Panel Visibility Binding")]
public class PanelVisibilityBinding : NguiBooleanBinding
{
	private bool _selfVisible;
	
	private static List<Renderer> _renderers;
	private static List<UIWidget> _uiWidgets;
	 
	private bool Visible
	{
		get 
		{
			return _selfVisible;
		}	
	}
	
	public override void Awake()
	{
		base.Awake();
	}
	
	protected override void ApplyNewValue(bool newValue)
	{
		_selfVisible = newValue;
		SetVisible(gameObject, Visible);
	}
	
	private static void SetVisible(GameObject gameObject, bool isVisible)
	{
		var uiPanels = gameObject.GetComponentsInChildren<UIPanel>();
		foreach (var uiPanel in uiPanels)
			uiPanel.enabled = isVisible;
	}
	
	
}
