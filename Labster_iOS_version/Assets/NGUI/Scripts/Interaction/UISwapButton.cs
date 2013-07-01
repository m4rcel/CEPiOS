//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2012 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Sample script showing how easy it is to implement a standard button that swaps sprites.
/// </summary>

[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/Image Button")]
public class UISwapButton : MonoBehaviour
{
	public UISprite TargetUISprite;
	public Collider TargetCollider;
	
	UISprite thisUISprite;
	Collider thisCollider;


	void Start ()
	{
		thisUISprite=gameObject.GetComponent<UISprite>();
		thisCollider=gameObject.GetComponent<Collider>();
	}

	public void IfPressed ()
	{
		if (TargetUISprite != null && TargetCollider != null)
		{
			TargetUISprite.enabled=true;
			TargetCollider.enabled=true;
			thisUISprite.enabled=false;
			thisCollider.enabled=false;
		}
	}
}