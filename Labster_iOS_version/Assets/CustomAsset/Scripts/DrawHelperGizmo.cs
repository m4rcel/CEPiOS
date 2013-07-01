using UnityEngine;
using System.Collections;

[AddComponentMenu("Gizmo/Helper Gizmo")]
public class DrawHelperGizmo : MonoBehaviour {
	
	public float Size = 1f ;
	public Color Helper_Color = new Color(0f,1f,0f,0.3f);

	void OnDrawGizmos(){
//        Gizmos.color = Helper_Color;
		Color col = Helper_Color;
		Gizmos.color = new Color(col.r, col.g, col.b,0.5f);
//		Gizmos.DrawCube(transform.position, new Vector3(Size, Size, Size));
//		Gizmos.DrawWireCube(transform.position, new Vector3(Size, Size, Size));
		Vector3 pos = transform.position;
		Gizmos.DrawLine (new Vector3(pos.x - (Size * 0.5f),pos.y,pos.z),new Vector3(pos.x + (Size * 0.5f),pos.y,pos.z));
		Gizmos.DrawLine (new Vector3(pos.x,pos.y - (Size * 0.5f),pos.z),new Vector3(pos.x,pos.y + (Size * 0.5f),pos.z));
		Gizmos.DrawLine (new Vector3(pos.x,pos.y,pos.z - (Size * 0.5f)),new Vector3(pos.x,pos.y ,pos.z + (Size * 0.5f)));
	}
	void OnDrawGizmosSelected(){
		Color col = Helper_Color;
		Gizmos.color = col;
//		Gizmos.color = new Color(col.r, col.g, col.b,1f);
		Gizmos.DrawCube(transform.position, new Vector3(Size, Size, Size));
		Gizmos.DrawWireCube(transform.position, new Vector3(Size, Size, Size));
	}

//	// Use this for initialization
//	void Start () {
//	
//	}
//	
//	// Update is called once per frame
//	void Update () {
//	
//	}
}
