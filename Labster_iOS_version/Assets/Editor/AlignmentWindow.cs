using UnityEngine;
using UnityEditor;

namespace Assets.Editor
{
    public class AlignmentWindow :EditorWindow {
        static GameObject _source;
        static GameObject _target;
	
        static bool _selectPosition = true;
        static bool _selectRotation = true;
        static bool _selectScale;
	
        [MenuItem("Align/Position")]
        static void Init () {
            _source = Selection.activeGameObject;
            // Get existing open window or if none, make a new one:
            var window = (AlignmentWindow)GetWindow (typeof (AlignmentWindow));
        }
    
        void OnGUI () {
            _source = (GameObject)EditorGUILayout.ObjectField("Source", _source, typeof(GameObject), true);
            _target = (GameObject)EditorGUILayout.ObjectField("Target", _target, typeof(GameObject), true);
		
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Position"); 	_selectPosition 	= EditorGUILayout.Toggle(_selectPosition);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Rotation"); 	_selectRotation 	= EditorGUILayout.Toggle(_selectRotation);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Scale"); 		_selectScale 	    = EditorGUILayout.Toggle(_selectScale);
            EditorGUILayout.EndHorizontal();

            if (!GUILayout.Button("Align!")) return;
            if (!_target)
                Debug.LogError("No Target GameObject specified, you have to specify a target GameObject first");
            else
            {
                if (_selectPosition)
                    _source.transform.position = _target.transform.position;
                if (_selectRotation)
                    _source.transform.rotation = _target.transform.rotation;
                if (_selectScale)
                    _source.transform.localScale = _target.transform.localScale;
            }
        }
    }
}
