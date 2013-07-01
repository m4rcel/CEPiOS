using UnityEngine;
using System.Collections;

public class DockPlaceholder : MonoBehaviour {
	
	public bool IsStatic;
	
	public DockPlaceholder PreviousDock;
	public DockPlaceholder NextDock;
	
	public bool IsLast{
		get{
			return (NextDock == null);
		}
	}
	public bool IsAvailable;
	
	private GameObject _containerGameObject;
	public GameObject ContainerGameObject{
		get{
			if (_containerGameObject == null)
				_containerGameObject = transform.FindChild("Container").gameObject;
			return _containerGameObject;
		}
	}
	private GameObject _dockGameObject;
	public GameObject DockGameObject{
		get{
			if (_dockGameObject == null)
				_dockGameObject = transform.FindChild("Dock").gameObject;
			return _dockGameObject;
		}
	}
	public bool DockIsVisible{
		get{
			return (DockGameObject.renderer.enabled);
		}
	}
	
	private int _prevChildCount = -1;
	
	private Transform _currentItem;
	public bool HasItem{
		get{
			return _currentItem != null;
		}
	}
	//private HighlightableObject _currentItemHighlight;
	
	void Start () {
		if (PreviousDock)
			PreviousDock.NextDock = this;
	}
	
	// Update is called once per frame
	void Update () {
		if (!IsStatic)
		{
//			var childCount = transform.childCount;
			var childCount = ContainerGameObject.transform.childCount;
			
			// recalculate X position to align with previous dock
			// this is to make sure there are no gaps between the docks
			if (PreviousDock)
				SlideLeft();
			
			// Update when child count changes
			if (_prevChildCount != childCount)
			{
				_currentItem = GetCurrentItem();
				//_currentItemHighlight = null;
				
				if (HasItem)
				{
					if (!IsAvailable)
						DockGameObject.animation.Play("Spawn");
					
					if (_prevChildCount != -1)
						StartCoroutine(ResizeChild(_currentItem));
					
					//_currentItemHighlight = _currentItem.GetComponent<HighlightableObject>();
					
//					if (!_currentItemHighlight)
//					{
//						_currentItemHighlight = _currentItem.gameObject.AddComponent<HighlightableObject>();
//					}
					//_currentItemHighlight.ConstantOnImmediate(Color.red);
				}
				else{
					NextDock = null;
					if (PreviousDock != null)
						DockGameObject.animation.Play("Hide");
				}
				
			}
			
			_prevChildCount = childCount;
			
			if (HasItem)
			{
				IsAvailable = false;
				
				if (PreviousDock)
					PreviousDock.NextDock = this;
				
				var tempPosition = _currentItem.position;
				tempPosition = DockGameObject.renderer.bounds.center;
				//tempPosition.y = _dockGameObject.renderer.bounds.max.y + ( currentItem.position.y - CalculateTotalBounds(currentItem).min.y );
				_currentItem.position = tempPosition;
				SetLayerRecursively(_currentItem.gameObject, gameObject.layer);
			}
			else {
				if (IsLast)
					CheckOthersAvailability();
			}
			
			if (PreviousDock && PreviousDock.PreviousDock && !PreviousDock.HasItem)
				PreviousDock = PreviousDock.PreviousDock;
			
			if (PreviousDock){
				var enabled = (HasItem || DockGameObject.animation.IsPlaying("Hide") || IsAvailable);
				DockGameObject.renderer.enabled = enabled;
				collider.enabled = enabled;
			}
			
			if (!HasItem && !DockGameObject.animation.IsPlaying("Hide"))
				RepositionToLast();
			
//			if (_currentItemHighlight)
//				_currentItemHighlight.EnableThis();
			
		}
		
		if (NextDock != null && NextDock.DockIsVisible == false)
			NextDock = null;
	}
	
	Transform GetCurrentItem(){
		Transform currentItem = null;
		if (ContainerGameObject.transform.childCount > 0)
			currentItem = ContainerGameObject.transform.GetChild(0);
		return currentItem;
//		Transform currentItem = null;
//		for (var i=0;i<transform.childCount;i++)
//		{
//			if (transform.GetChild(i).gameObject != DockGameObject)
//			{
//				currentItem = transform.GetChild(i);
//				break;
//			}
//		}
//		return currentItem;
	}
	
	void SlideLeft()
	{
		var currentPosition = transform.position;
		currentPosition.x += (PreviousDock.DockGameObject.renderer.bounds.max.x + 0.001f - currentPosition.x)/10;
		transform.position = currentPosition;
	}
	
	void RepositionToLast()
	{
		var rootDock = transform.parent;
		if (rootDock != null)
		{
			for (var i=0; i<rootDock.childCount; i++)
			{
				var childDock = rootDock.GetChild(i).GetComponent<DockPlaceholder>();
				if (childDock.IsLast && childDock != this && childDock.DockIsVisible)
				{
					PreviousDock = childDock;
					NextDock = null;
				}
			}
		}
	}
	void CheckOthersAvailability()
	{
		var rootDock = transform.parent;
		if (rootDock != null)
		{
			var IsOther = false;
			for (var i=0; i<rootDock.childCount; i++)
			{
				var childDock = rootDock.GetChild(i).GetComponent<DockPlaceholder>();
				if (childDock && childDock.IsAvailable)
				{
					IsOther = true;
				}
			}
			if (!IsOther)
			{
				IsAvailable = true;
				DockGameObject.animation.Play("Spawn");
			}
		}
	}
	
	void SetLayerRecursively(GameObject target, int layerIndex)
	{
		target.layer = layerIndex;
		for (var i=0; i<target.transform.childCount; i++){
			var child = target.transform.GetChild(i).gameObject;
			SetLayerRecursively(child, layerIndex);
		}
	}
	
	IEnumerator ResizeChild(Transform target)
	{
		var currentScale = target.localScale.x;
		//var targetScale = 0.5f * currentScale;
		var targetScale = CalculateItemScale(target, 0.8f, DockGameObject.renderer.bounds.size);
		while (currentScale > targetScale) {
			currentScale = Mathf.Lerp(currentScale, targetScale, Time.deltaTime*8f);
			target.localScale = new Vector3(currentScale, currentScale, currentScale);
			yield return null;
		}
	}
	
	#region Item Resizing Algorithm
		// Calculate the correct item scale
		// if current * target is bigger than maximumLimit;
		// then we use maximumLimit
		float CalculateItemScale(Transform target, float factor, Vector3 maximumLimit)
		{
			var currentBounds = CalculateTotalBounds(target).size;
			var predictedBounds = currentBounds * factor;
			var targetScale = target.localScale.x * factor;
				targetScale = (predictedBounds.x > maximumLimit.x) ? (maximumLimit.x * 0.9f / currentBounds.x) : targetScale;
				targetScale = (predictedBounds.z > maximumLimit.z) ? (maximumLimit.z * 0.9f / currentBounds.z) : targetScale;
			return targetScale;
		}
		// Sums up total bounds of a target, including all child objects
		// this is based on unity's renderer.bounds
		Bounds CalculateTotalBounds(Transform target)
		{
			var allRenderers = target.GetComponentsInChildren<Renderer>(true);
			var maxSize = Vector3.zero;
			var totalCenter = Vector3.zero;
			
			// loop through all renderers
			var tempBounds = allRenderers[0].bounds;
			for (var i=1; i<allRenderers.Length; i++){
				var renderer = allRenderers[i];
				// Encapsulate : Grow the bounds to encapsulate the bounds.
				tempBounds.Encapsulate(renderer.bounds);
			}
			return tempBounds;
		}
	#endregion
}
