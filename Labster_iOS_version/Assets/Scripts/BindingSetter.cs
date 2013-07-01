using UnityEngine;
using System.Collections;

public class BindingSetter : MonoBehaviour {
	
	public EZData.Context Context;
	public NguiRootContext Root;
	
	// Use this for initialization
	void Start () {
		var context = gameObject.GetComponent<NguiDataContext>();
		context.SetContext(Context, Root);
	}
}
