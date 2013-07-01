using System.Linq;
using UnityEngine;
using UnityEditor;

namespace Assets.Editor
{
    public class Duplicator {
        [MenuItem("Edit/DuplicateAuto %#d")]	
        static void ExportResource ()
        {		
            GameObject newObject 	= null, oldObject 	= null;
            Transform  newTransform = null, oldTransform = null;
		
            var prefabRoot = PrefabUtility.GetPrefabParent (Selection.activeGameObject);
            if (prefabRoot != null)
                newObject = (GameObject)PrefabUtility.InstantiatePrefab (prefabRoot);
            else
                newObject = (GameObject)Object.Instantiate (Selection.activeGameObject);
		
            newTransform = newObject.transform;
		
            oldObject = Selection.activeGameObject;
            oldTransform = Selection.activeGameObject.transform;
		
            newObject.name = Selection.activeGameObject.name;		
            var materialList = oldObject.GetComponentsInChildren<Renderer>().ToDictionary(renderer => renderer.name, renderer => renderer.sharedMaterial);

            foreach (var renderer in newObject.GetComponentsInChildren<Renderer>())
                renderer.material = materialList[renderer.gameObject.name];
		
            newObject.name = ProduceName(newObject.name);
            newTransform.SetTransform(oldTransform);
		
            Selection.activeObject = newObject;
        }	
	
        static string ProduceName(string inString)
        {
            var outString		= "";
            var numeration		= 0;		
            var parseSuccess	= false;
		
            //delimit string using underscore
            var splitString = inString.Split("_" [0]);		
            if(splitString.Length > 0)
                parseSuccess = int.TryParse(splitString[splitString.Length-1], out numeration);
		
            //add original name in the front		
            outString += splitString[0]+"_";
            for (var i=1;i<splitString.Length-(parseSuccess?1:0);i++)
                outString+=splitString[i]+"_";
		
            //add numeration
            outString += (numeration+1).ToString();
		
            if(Selection.activeGameObject.transform.parent)
                if(Selection.activeGameObject.transform.parent.FindChild(outString))
                    outString = ProduceName(outString);
		
            return outString;
        }
    }

    public static class UnityExtension
    {
        public static Transform SetTransform(this Transform value, Transform newTransform)
        {
            value.parent = newTransform.parent;
            value.position = newTransform.position;
            value.rotation = newTransform.rotation;
            value.localScale = newTransform.localScale;
		
            return value;
        }
    }
}