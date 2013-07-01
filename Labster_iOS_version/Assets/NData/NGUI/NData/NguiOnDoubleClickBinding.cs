using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("NGUI/NData/OnDoubleClick Binding")]
public class NguiOnDoubleClickBinding : NguiCommandBinding
{
	private DateTime m_tappedTime = DateTime.MinValue;
	
	public double duration = 400.0;
	
	public override void Awake()
	{
		base.Awake();
	}
	
	public void OnClick()
	{
		DateTime now = DateTime.Now;

		double diff = (now - m_tappedTime).TotalMilliseconds;
		if (diff < duration && _command != null)
		{
			_command.DynamicInvoke();
		}
		
		m_tappedTime = now;
	}	
}
