using UnityEngine;
using System.Collections;

public class ChangeTextureUsingKey : MonoBehaviour {
	
	public KeyCode key;
	public Texture texture;
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(key)){
			renderer.material.mainTexture = texture;
		}
	}
}
