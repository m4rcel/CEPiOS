using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("NGUI/NData/OnClick Binding")]
public class NguiOnClickBinding : NguiCommandBinding
{
	void OnClick()
	{
		UpdateBinding();
		
		if (_command == null)
		{
			return;
		}
		
		_command.DynamicInvoke();
	}
}
