using UnityEngine;
using System.Collections;

public class ParentChecker : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		renderer.enabled = (transform.parent != null);
	}
}
