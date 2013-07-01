using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class NguiItemDataContext : NguiDataContext
{
	public NguiItemsSourceBinding ItemsSource { get; private set; }
	
	public event System.Action OnSelectedChange;
	
	private bool _selected;
	
	public EZData.Context shownContext;
	
	public bool Selected
	{
		get { return _selected; }
		private set
		{
			bool needUpdate = (value != _selected) && (OnSelectedChange != null);
			_selected = value;
			if (needUpdate)
				OnSelectedChange();
		}
	}
	public int Index { get; private set; }
	
	void Update()
	{
		if (ItemsSource == null)
			ItemsSource = NguiUtils.GetComponentInParents<NguiItemsSourceBinding>(gameObject);
	}
	
	void OnClick()
	{
		if (ItemsSource != null)
			ItemsSource.OnSelectionChange(gameObject);
	}
	
	public void SetSelected(bool selected)
	{
		Selected = selected;
	}
	
	public void SetIndex(int index)
	{
		Index = index;
	}
	
	public void SetContext(EZData.Context c, NguiRootContext rootContext)
	{
		_context =  c;
		shownContext = c;
		_root = rootContext;
		
		var bindings = gameObject.GetComponentsInChildren<NguiBinding>();
		foreach (var binding in bindings)
		{
			binding.SetContext(this);
		}
		
		var multiBindings = gameObject.GetComponentsInChildren<NguiMultiBinding>();
		foreach (var binding in multiBindings)
		{
			binding.SetContext(this);
		}
	}
}
