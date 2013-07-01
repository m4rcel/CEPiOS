using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class NguiMultiBinding : MonoBehaviour, EZData.IBinding
{
	public List<string> Paths;
	
	public IList<string> ReferencedPaths { get { return Paths; } }
	
	protected NguiDataContext _context;

	public virtual void Awake()
	{
		_context = NguiUtils.FindRootContext(gameObject);
	}
	
	public void OnContextChange()
	{
		UpdateBinding();
	}
	
	public virtual void UpdateBinding()
	{
	}
	
	public void SetContext(NguiDataContext c)
	{
		_context = c;
	}
	
	public NguiDataContext GetContext()
	{
		return _context;
	}
	
	public virtual void Start()
	{
		UpdateBinding();
	}
}
