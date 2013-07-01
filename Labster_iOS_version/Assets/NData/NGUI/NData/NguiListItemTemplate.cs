using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("NGUI/NData/List Item Template")]
public class NguiListItemTemplate : MonoBehaviour
{
	public GameObject Template;
	public string xmlPath = "";
	
	private NguiDataContext _rootContext = null;
		
	void Awake()
	{
		_rootContext = NguiUtils.FindRootContext(gameObject);
	}
	
	public GameObject Instantiate(EZData.Context itemContext, int index)
	{
		if (Template == null)
		{
			return null;
		}
		
		GameObject instance = (GameObject)Instantiate(Template);
		var itemData = instance.AddComponent<NguiItemDataContext>();
		itemData.SetContext(itemContext, _rootContext.Root);
		itemData.SetIndex(index);
		
		//load the template from xml
		instance.AddComponent<UIDragPanelContents>();
		
		return instance;
	}
}
