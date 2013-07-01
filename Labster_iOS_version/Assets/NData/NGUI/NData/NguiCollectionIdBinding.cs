using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("NGUI/NData/ItemsSource Binding")]
public class NguiCollectionIdBinding : NguiBinding
{
	private NguiListItemTemplate _itemTemplate;
	protected EZData.Collection _collection;
	protected bool _isCollectionSelecting = false;
	
	private UITable _uiTable = null;
	
	public string CollectionId;
	
	public override void Awake()
	{
		base.Awake();
		_uiTable = GetComponent<UITable>();
		_itemTemplate = gameObject.GetComponent<NguiListItemTemplate>();
	}
	
	public override void UpdateBinding()
	{
		if (_collection != null)
		{
			_collection.OnSelectionChange -= OnCollectionSelectionChange;
			_collection = null;
		}
			
		var context = GetContext();
		if (context == null)
			return;
		
		_collection = context.FindCollection(Path, this);
		if (_collection == null)
			return;		
		
		
		for (var i = 0; i < _collection.ItemsCount; ++i){
			var item = _collection.GetBaseItem (i);
			string itemId = item.FindProperty<string>("Id", null).GetValue();
			if(itemId==CollectionId){
				var dataContext = gameObject.AddComponent<NguiItemDataContext>();
					dataContext.SetContext(item, NguiUtils.FindRootContext(gameObject).Root);
				return;
			}
		}
		
	}
	
	protected virtual void OnCollectionSelectionChange()
	{
		for (var i = 0; i < transform.childCount; ++i)
		{
			var child = transform.GetChild(i).gameObject;
			int childNumber;
			if (int.TryParse(child.name, out childNumber))
			{
				var itemData = child.GetComponent<NguiItemDataContext>();
				if (itemData != null)
					itemData.SetSelected(childNumber == _collection.SelectedIndex);
			}
		}
	}
}
