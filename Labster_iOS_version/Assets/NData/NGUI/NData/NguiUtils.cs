using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NguiUtils
{
	public static NguiDataContext FindRootContext(GameObject gameObject)
	{
		NguiDataContext context = null; 
		var p = gameObject.transform.parent == null ? null : gameObject.transform.parent.gameObject;
		while (p != null && context == null)
		{
			context = p.GetComponent<NguiDataContext>();
			p = (p.transform.parent == null) ? null : p.transform.parent.gameObject;
		}
		return context;
	}
	
	public static T GetComponentInParents<T>(GameObject gameObject)
		where T : Component
	{
		T component = null;
		var p = gameObject;
		while (p != null && component == null)
		{
			component = p.GetComponent<T>();
			p = (p.transform.parent == null) ? null : p.transform.parent.gameObject;
		}
		return component;
	}
}
