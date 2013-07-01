using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("NGUI/NData/Function Binding")]
public class NguiFunctionBinding : NguiBinding
{
	public string Parameter1;
	public string Parameter2;
	
	void CallCommand()
	{	
		var context = GetContext();
		if (context == null)
			return;
		
		if(!string.IsNullOrEmpty(Path)){
			var methodInfo = context.RootViewModel.GetType().GetMethod(Path);
			
			
			var uiLabel = transform.GetComponentInChildren<UILabel>();
			
			methodInfo.Invoke(context.RootViewModel, new []{Parameter1, uiLabel.text});
		}		
	}
}
