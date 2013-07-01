using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    public class ExportAssetBundles {
        [MenuItem("Assets/Build AssetBundle From Selection - Track dependencies")]
        static void ExportResource () {
            // Bring up save panel
            var path = EditorUtility.SaveFilePanel ("Save Resource", "", "New Resource", "unity3d");
            if (path.Length == 0) return;
            // Build the resource file from the active selection.
            var selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
            Validate(((GameObject)Selection.activeObject).transform);
            BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets);
            Selection.objects = selection;
        }

        private static void Validate(Transform target)
        {
			Debug.Log("Start : "+target.name);
            var animations = target.GetComponentsInChildren<Animation>();
            foreach (var animation in animations)
                animation.playAutomatically = false;

            var colliders = target.GetComponentsInChildren<Collider>();
			Debug.Log("Found "+colliders.Length);
            foreach (var collider in colliders)
			{
				if (collider.enabled)
				{
					Debug.Log("Collider deactivated on "+collider.gameObject.name);
                	collider.enabled = false;
				}
			}
			Debug.Log("Finish : "+target.name);
        }
    }

}