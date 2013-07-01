using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

[System.Serializable]
public class NguiBinding : MonoBehaviour, EZData.IBinding
{
	public string Path;

	public NguiItemDataContext dataContext;
	
	public IList<string> ReferencedPaths { get { return new List<string> { Path }; } }
	
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
	public void SetContext(EZData.Context c, NguiRootContext root)
	{
		_context.SetContext(c, root);
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
