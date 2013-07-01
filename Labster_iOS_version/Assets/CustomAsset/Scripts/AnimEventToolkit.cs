using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System;

public class AnimEventToolkit : MonoBehaviour {
	
	public void ChangeActiveCamera(string inputParameter){
		// example : (a,b) as GameObject
		//where "a" is camera will  be deacivate and "b" is camera will be activate
		//GameObject.Find("Main Camera").active=false;

		
		string[] param = inputParameter.Split(',');
		GameObject.Find(param[0]).GetComponent<Camera>().enabled =false; //disable camera (first param)
		GameObject.Find(param[1]).GetComponent<Camera>().enabled =true; //disable camera (secondparam)
		
		//GameObject.Find("Main Camera").GetComponent<Camera>().enabled=false; // enable camera ( second param)
	}
	
	public void PlayScene (string inputParameter){
		// just put the name of the object that you want to play 
		// on the next scene
		string[]param = inputParameter.Split(',');
		GameObject.Find(param[0]).GetComponent<Animation>().Play();

	}
	
	
	public void ChangeRenderer(string inputParameter){
		// example : (True) as boolean
		// just put this script in the GO and will  work for entire child.
		
		string[] param = inputParameter.Replace(" ", "").Split(',');
		GameObject.Find(param[0]).renderer.enabled=bool.Parse(param[0]);
		
	 	var renderers = new List<Renderer>();
		renderers.AddRange(gameObject.GetComponentsInChildren<Renderer>());
		
		foreach (var renderer in renderers)
			renderer.enabled = bool.Parse( inputParameter[0].ToString());
		
	}
		
	

	public void SetColor(string inputParameter){
		
		
				
		// example :(PN,R,G,B,A,D)  PropertyName,Red,Green,Blue,Alpha,Duration
		string[] param = inputParameter.Replace(" ", "").Split(',');
		string propertyName = param[0];
		Color TargetColor = new Color(
							float.Parse(param[1]), // Red
							float.Parse(param[2]), // Green
							float.Parse(param[3]), // Blue
							float.Parse(param[4]));// Alpha
		float Duration = 	float.Parse(param[5]); // Duration
		
		gameObject.renderer.material.SetColor(propertyName,TargetColor);
		
		var renderers = new List<Renderer>();
		renderers.AddRange(gameObject.GetComponentsInChildren<Renderer>());
		
		foreach (var renderer in renderers)
			renderer.material.SetColor(propertyName,TargetColor);
		
	}
	
	
	
	public IEnumerator AnimateSetFloat(string inputParameter){
						
		// (PropertyName,value,Duration,MaterialName)
		
		string[] param = inputParameter.Replace(" ", "").Split(',');
		float currentValue= gameObject.renderer.material.GetFloat(param[0]);
		float delta=float.Parse(param[1])/float.Parse(param[2]);
	
		//collect whole children data
		var renderers = new List<Renderer>();
		renderers.AddRange(gameObject.GetComponentsInChildren<Renderer>());
			
			
		while (currentValue< float.Parse(param[1])){
		
			foreach (var renderer in renderers){
				if (renderer.material.name==param[3].ToString() ){
					renderer.material.SetFloat(param[0],float.Parse(param[1]));
				}
			}
			currentValue+=delta;
			yield return null;
		}
		
		
		
		
	}
	
	
	
	public void PhaseCompleted(string phaseId){
		Debug.Log ("Anim phase completed: " + phaseId);	
	}
	
	
	
	public void ShowTitle(string titleId){
		Debug.Log ("Show title: " + titleId);	
	}
	
	public void HideTitle(){
		Debug.Log ("Hide title");	
	}
	
	
	
	public void ShowConversation(string conversationId){
		Debug.Log ("Show Conversation: " + conversationId);	
	}
	
	public void ShowMessage(string messageId){
		Debug.Log ("Message: " + GetMessageFromId(messageId) + " (ID: " + messageId + ")");	
	}
	public void HideMessage(){
		Debug.Log ("Hide Message");	
	}
	
		
	
	public string GetMessageFromId(string messageId){
		switch (messageId){
			case "Message1": return "The DNA denatures and becomes single-stranded";
			case "Message2": return "Test...";
			case "Message3": return "Bla bla";
			case "Message4": return "This is message 1";
			case "Message5": return "This is message 1";
			case "Message6": return "This is message 1";
			case "Message7": return "This is message 1";
			
			default: return "(none)";
			
		}
		
	}
	
	
	public void Completed(){
		
		Debug.Log ("Anim ended");	
	}
	
		
	public void Pause(){
		
		Debug.Log ("Pause");	
	}
	
	public void SetSpeed(string speed){
		Debug.Log ("SetSpeed: " + speed);
	}
	
		
		
//	
//	IEnumerator AnimatorMaterial(string[] param,float duration)
//	{
//	Mathf.Lerp(
//	  
//	
//		
//			
//	}

	

}
