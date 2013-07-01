using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent (typeof(BoxCollider))]
[AddComponentMenu("Gizmo/Trigger Gizmo")]
public class TriggerGizmo : MonoBehaviour {
	
	public Color Helper_Color = new Color(0f,1f,0f,0.3f);
	
	
	void OnDrawGizmos(){
		Color col = Helper_Color;
		Gizmos.color = new Color(col.r, col.g, col.b,col.a);
		Gizmos.DrawCube(collider.bounds.center,collider.bounds.size);
		Gizmos.DrawWireCube(collider.bounds.center,collider.bounds.size);
	}

}
