using UnityEngine;

public class EndAnimationTrigger : MonoBehaviour {
	public GameObject root;
	
	void EndAnim () {
		
		root.active=false;
	}
	
	void Notification(string notification){
		Debug.Log(notification);
	}
}
