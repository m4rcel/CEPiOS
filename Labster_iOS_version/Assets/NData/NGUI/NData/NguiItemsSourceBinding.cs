using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("NGUI/NData/ItemsSource Binding")]
public class NguiItemsSourceBinding : NguiBinding
{
	private NguiListItemTemplate _itemTemplate;
	protected EZData.Collection _collection;
	protected bool _isCollectionSelecting = false;
	
	private UITable _uiTable = null;
	
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
			_collection.OnItemInsert -= OnItemInsert;
			_collection.OnItemRemove -= OnItemRemove;
			_collection.OnItemsClear -= OnItemsClear;
			_collection.OnSelectionChange -= OnCollectionSelectionChange;
			_collection = null;
		}
			
		var context = GetContext();
		if (context == null)
			return;
		
		_collection = context.FindCollection(Path, this);
		if (_collection == null)
			return;
	
		_collection.OnItemInsert += OnItemInsert;
		_collection.OnItemRemove += OnItemRemove;
		_collection.OnItemsClear += OnItemsClear;
		_collection.OnSelectionChange += OnCollectionSelectionChange;
		
		
		
		// Add any already existing items
		for (var i=0; i<_collection.ItemsCount; i++){
			var item = _collection.GetBaseItem (i);
			OnItemInsert(i, item);
		}
	}
	
	protected virtual void OnItemInsert(int position, EZData.Context item)
	{
		if (_itemTemplate != null && _uiTable != null)
		{
			var itemName = position.ToString();
			
			var itemKey = item.FindProperty<string>("Key", null).GetValue();
			if(!string.IsNullOrEmpty(itemKey))
				itemName = itemKey;
			
			var itemObject = _itemTemplate.Instantiate(item, position);
			itemObject.name = string.Format("{0}", itemName);
			for (var i = 0; i < transform.childCount; ++i)
			{
				var child = transform.GetChild(i).gameObject;
				int childNumber;
				if (int.TryParse(child.name, out childNumber) && childNumber >= position)
				{
					child.name = string.Format("{0}", childNumber + 1);
				}
			}
			itemObject.transform.parent = gameObject.transform;
			itemObject.transform.localScale = Vector3.one;
			itemObject.transform.localPosition = Vector3.back;
			itemObject.active=gameObject.active;
			
			foreach(var dragObject in itemObject.GetComponentsInChildren<UIDragObject>())
			{
				if (dragObject.target == null)
					dragObject.target = gameObject.transform;
			}
			foreach(var dragObject in itemObject.GetComponents<UIDragObject>())
			{
				if (dragObject.target == null)
					dragObject.target = gameObject.transform;
			}
			
			foreach(var tweenScale in itemObject.GetComponentsInChildren<TweenScale>())
			{
				tweenScale.SendMessage("Awake");
			}
			foreach(var tweenScale in itemObject.GetComponents<TweenScale>())
			{
				tweenScale.SendMessage("Awake");
			}
						
			_uiTable.repositionNow = true;
			
		}
	}
	
	protected virtual void OnItemRemove(int position)
	{
		for (var i = 0; i < transform.childCount; ++i)
		{
			var child = transform.GetChild(i).gameObject;
			int childNumber;
			if (int.TryParse(child.name, out childNumber))
			{
				if (childNumber == position)
				{
					GameObject.Destroy(child);
					break;
				}
			}
		}
		for (var i = 0; i < transform.childCount; ++i)
		{
			var child = transform.GetChild(i).gameObject;
			int childNumber;
			if (int.TryParse(child.name, out childNumber))
			{
				if (childNumber > position)
				{
					child.name = string.Format("{0}", childNumber - 1);
				}
			}
		}
	
		if (_uiTable != null)
			_uiTable.repositionNow = true;

	}
	
	protected virtual void OnItemsClear()
	{
		if (this==null){
			UpdateBinding();	
			return;
		}
		for (var i=transform.childCount - 1; i>=0; i--){
			GameObject.Destroy(transform.GetChild(i).gameObject);
		}
	}
	
	public void OnSelectionChange(GameObject selectedObject)
	{
		if (_collection != null && !_isCollectionSelecting)
		{
			_isCollectionSelecting = true;
			for (var i = 0; i < transform.childCount; ++i)
			{
				var child = transform.GetChild(i).gameObject;
				if (selectedObject != child)
					continue;
				int childNumber;
				if (int.TryParse(child.name, out childNumber))
				{
					_collection.SelectItem(childNumber);
					break;
				}
			}
			_isCollectionSelecting = false;
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
