using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class NguiCommandBinding : NguiBinding
{
	protected System.Delegate _command;
	
	public override void UpdateBinding()
	{
		_command = null;
		
		var context = GetContext();
		if (context == null)
			return;
		
		_command = context.FindCommand(Path, this);
	}
}
