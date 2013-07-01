using System.Linq;
using UnityEngine;
using UnityEditor;

namespace Assets.Editor
{
    public class GoRenamerCamelCase {
        [MenuItem("Build/Rename: CamelCase")]
        static void RenamerCamelCase()
        {
            var objects = Object.FindSceneObjectsOfType(typeof (GameObject));
            foreach (var go in objects.Cast<GameObject>())
            {
                go.name = RenameToCamelCase(go.name);
            }
        }

        static string RenameToCamelCase(string name){
            var outString = "";
            for(var i=0;i<name.Length;i++){
                if(i==0){
                    if(name[i]!='_')
                        outString += name[i].ToString().ToUpper();
                    else
                        outString += name[i].ToString();
                }
                else{
                    if(name[i]!='_' && name[i]!=' ')
                        outString += name[i - 1] == '_' || name[i - 1] == ' ' ? name[i].ToString().ToUpper() : name[i].ToString();
                    else if (i + 1 < name.Length && char.IsNumber(name[i + 1]))
                        outString += name[i].ToString();
                }
            }
		
            return outString;
        }
    }
}
