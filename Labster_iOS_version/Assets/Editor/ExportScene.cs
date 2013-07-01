using System.Linq;
using UnityEngine;
using UnityEditor;

using System.IO;
using System.Collections.Generic;

namespace Assets.Editor
{
    public class exportScene
    {
        [MenuItem("Build/Create Scene AssetBundle")]
        static void MyBuild(){
            Validate();

            var levels = new[] {(EditorApplication.currentScene)};
            var scenePath = EditorApplication.currentScene;
            var lastSlash = 0;
            var lastDot = 0;
	    
            for(var i=0; i<scenePath.Length; i++){
                if(scenePath[i] == '/') lastSlash=i;
                if(scenePath[i] == '.') lastDot=i;
            }
	    
            var sceneName = scenePath.Substring(lastSlash+1,lastDot-lastSlash-1);
	    
            var path = EditorUtility.SaveFilePanel ("Save Resource", "", sceneName, "unity3d");

            if (!string.IsNullOrEmpty(path))
                BuildPipeline.BuildStreamedSceneAssetBundle(levels, path, BuildTarget.WebPlayer);

            var sr = File.CreateText(sceneName+".xml");
                sr.WriteLine(CreateSummary());
                sr.Close();
        }

        private static void Validate()
        {
            var gameObjects = Object.FindObjectsOfType(typeof (GameObject));
            var usedMaterialList = new Dictionary<Material, List<string>>();

            foreach (var child in gameObjects)
            {
                var childGameObject = ((GameObject) child);
                // Look for collider(s) and disable it, the default is disabled
                var animationComponent = childGameObject.GetComponent<Animation>();
                if (animationComponent != null && animationComponent.playAutomatically)
                {
                    Debug.Log(string.Format("Disable Animation in GameObject ({0})", child.name));
                    animationComponent.playAutomatically = false;
                }
                // Look for collider(s) and disable it, the default is disabled
                var colliderComponent = childGameObject.GetComponent<Collider>();
                if (colliderComponent!=null && colliderComponent.enabled)
                {
                    Debug.Log(string.Format("Disable Collider in GameObject ({0})", child.name));
                    colliderComponent.enabled = false;
                }
                // Look for audioListener(s) and remove it, as we never need audioListeners
                var audioListenerComponent = childGameObject.GetComponent<AudioListener>();
                if (audioListenerComponent != null)
                {
                    Debug.Log(string.Format("Removed AudioListener in GameObject ({0})", child.name));
                    Object.DestroyImmediate(audioListenerComponent);
                }
            }
        }

        static string CreateSummary()
        {
            var gameObjects = Object.FindObjectsOfType(typeof(GameObject));
            var usedMaterialList = new Dictionary<Material, List<string>>();
            var result = "";
            foreach (var child in gameObjects)
            {
                var childGameObject = ((GameObject)child);
                if (childGameObject.renderer == null) continue;
                foreach (var material in childGameObject.renderer.sharedMaterials)
                {
                    if (!usedMaterialList.ContainsKey(material))
                        usedMaterialList.Add(material, new List<string>());

                    Debug.Log(childGameObject.transform.GetPath());
                    usedMaterialList[material].Add(childGameObject.transform.GetPath());
                }
            }

            // Generate List of materials used
            foreach (var key in usedMaterialList.Select(material => material.Key))
            {
                result += string.Format("{0} ({1})\n", key.name, (key.mainTexture == null ? "No Texture" : key.mainTexture.name) );
                usedMaterialList[key].Sort();
                result = usedMaterialList[key].Aggregate(result, (current, gameObjectName) => current + string.Format("\t{0}\n", gameObjectName));
            }

            return result;
        }
    }
	
}

    