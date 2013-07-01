using UnityEngine;
using System.Collections;

public class SwapMaterialByTime : MonoBehaviour {
	
	[System.Serializable]
	public class textureFrame{		
		public string strTime;
		public Material material;
		
		private float time;
		private bool enabled = true;
		
		public void Activate(){
			string[] timeParse;
			timeParse = strTime.Split(":" [0]);
			time = float.Parse(timeParse[0]) + ( (timeParse.Length>1)? (float.Parse(timeParse[1])/60) : 0 );
		}
		
		public bool CheckFrameTime(float timeNow){
			if(enabled){
				if(timeNow>=time){
					enabled=false;
					return true;
				}
				else
					return false;
			}
			else
				return false;
		}
	}
	
	[System.Serializable]
	public class lerpValue{
		public string startTime;
		public float duration;
		public float targetValue;		
		public string targetProperty;
		
		private float time;
		private bool enabled = true;
		
		public void Activate(){
			string[] timeParse;
			timeParse = startTime.Split(":" [0]);
			time = float.Parse(timeParse[0]) + ( (timeParse.Length>1)? (float.Parse(timeParse[1])/60) : 0 );
		}
		
		public bool CheckFrameTime(float timeNow){
			if(enabled){
				if(timeNow>=time){
					enabled=false;
					return true;
				}
				else
					return false;
			}
			else
				return false;
		}
	}
	
	IEnumerator animateShaderProperty(float duration, float targetValue, string property){
		
		float initValue = 0;
		
		foreach(var child in childList){
			if(child.renderer.material.HasProperty(property)){
				initValue =  child.renderer.material.GetFloat(property);
				break;
			}
		}
		
		float initTime=0;
		
		do {
			duration-=Time.deltaTime;
			initTime+=Time.deltaTime;
			foreach(var child in childList){
				if(child.renderer.material.HasProperty(property)){
					var tempValue = Mathf.Lerp (initValue, targetValue, initTime);
					child.renderer.material.SetFloat(property, tempValue);
					
					if(property=="_Trans"){
						if(tempValue<0.1f)
							child.renderer.enabled=false;
						
						if(tempValue>=0.1f)
							child.renderer.enabled=true;
					}
				}
			}
			yield return null;
		} while (duration>0);
		
		foreach(var child in childList){
			if(child.renderer.material.HasProperty(property)){
				child.renderer.material.SetFloat(property, targetValue);
			}
		}
	}
	
	
	public textureFrame[] TextureFrames;
	public lerpValue[] LerpValues;
	
	private Renderer[] childList;	
	
	void Awake(){
		childList =  gameObject.GetComponentsInChildren<Renderer>();
		foreach(var _textureFrame in TextureFrames){
			_textureFrame.Activate();
		}
		foreach(var _lerpValue in LerpValues){
			_lerpValue.Activate();
		}
	}
	
	void Update () {


		for(var i=0;i<TextureFrames.Length;i++){
			if(TextureFrames[i].CheckFrameTime(Time.time)){
				foreach(var child in childList){
					child.renderer.material = TextureFrames[i].material;
				}
			}
		}


		for(var i=0;i<LerpValues.Length;i++){
			var lerpValue = LerpValues[i];
			if(lerpValue.CheckFrameTime(Time.time)){
				StartCoroutine(animateShaderProperty(lerpValue.duration, lerpValue.targetValue, lerpValue.targetProperty));
			}
		}
	}
}
