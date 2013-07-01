using UnityEngine;

public class AnimationTriggerSwapObject : MonoBehaviour {
	public GameObject target1,target2;
	
	
	void Swap(){
		ChangeRendererRecursive(target1, false);
		ChangeRendererRecursive(target2, true);
	}
	
	  void ChangeRendererRecursive(GameObject Swap,bool isVisible){
       
               Renderer[] renderers = Swap.GetComponentsInChildren<Renderer>();
                       foreach (Renderer mr in renderers) {
                           mr.enabled = isVisible;
                       }
       }
 
	
}
