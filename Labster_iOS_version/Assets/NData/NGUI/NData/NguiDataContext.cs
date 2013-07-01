using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class NguiDataContext : MonoBehaviour
{
	protected EZData.Context _context;
	protected NguiRootContext _root;
	
	public void SetContext(EZData.Context c, NguiRootContext root){
		_context = c;
		_root = root;
	}
	
	
	public NguiRootContext Root
	{
		get
		{
			return _root ?? (this as NguiRootContext);
		}
	}
	
	public EZData.Property<T> FindProperty<T>(string path, EZData.IBinding binding)
	{
		if (string.IsNullOrEmpty(path))
			throw new Exception(string.Format("Trying to get a Binding property, but no property Path specified. GameObject name: '{0}'", gameObject.name));			
		
		if (_context == null)
		{
			return null;
		}
		try
		{
			return _context.FindProperty<T>(path, binding);
		}
		catch(Exception ex)
		{
			Debug.LogWarning("Failed to find property " + path + "\n" + ex);
			return null;
		}
	}
	
	public EZData.Property<int> FindEnumProperty(string path, EZData.IBinding binding)
	{
		if (_context == null)
		{
			return null;
		}
		try
		{
			return _context.FindEnumProperty(path, binding);
		}
		catch(Exception ex)
		{
			Debug.LogError("Failed to find enum property " + path + "\n" + ex);
			return null;
		}
	}
	
	public System.Delegate FindCommand(string path, EZData.IBinding binding)
	{
		if (_context == null)
		{
			return null;
		}
		try
		{
			if(string.IsNullOrEmpty(path))
				return null;
			return _context.FindCommand(path, binding);
		}
		catch(Exception ex)
		{
			Debug.LogError("Failed to find command (with no arguments) '" + path + "'\n" + ex);
			
			return null;
		}
	}
	
	public EZData.Collection FindCollection(string path, EZData.IBinding binding)
	{
		if (_context == null)
		{
			return null;
		}
		try
		{
			return _context.FindCollection(path, binding);
		}
		catch(Exception ex)
		{
			Debug.LogError("Failed to find collection " + path + "\n" + ex);
			return null;
		}
	}
	
	public EZData.Context RootViewModel {
		get { return _context; }
	}
}

