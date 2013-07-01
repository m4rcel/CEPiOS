using System.Linq;
using UnityEngine;
using UnityEditor;

namespace Assets.Editor
{
    public class GoRenamerRemovePrefixWithColon {
        [MenuItem("Build/Rename: Remove Prefix With Colon (:)")]
        static void RenamerCamelCase()
        {
            var gameObjects = Selection.gameObjects;
            foreach (
                var go in
                    gameObjects.Select(o => o.GetComponentsInChildren<Transform>(true))
                               .SelectMany(transforms => transforms.Select(t => t.gameObject)))
                go.name = RemovePrefixWithColon(go.name);
        }

        static string RemovePrefixWithColon(string name){
            return name.Substring(name.IndexOf(':') + 1, name.Length - name.IndexOf(':') - 1);
        }
    }
}
