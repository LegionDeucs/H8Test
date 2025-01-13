using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace MyCore.BuildIncrementor
{
    [CustomEditor(typeof(BuildVersionSettings))]
    public class BuildVersionSettingsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Editor"))
            {
                BuildVersionSettingsEditorWindow.Open(target as BuildVersionSettings);
            }
        }

        [OnOpenAsset]
        public static bool OpenEditor(int instanceID, int line)
        {
            var obj = EditorUtility.InstanceIDToObject(instanceID) as BuildVersionSettings;
            if (obj != null)
            {
                BuildVersionSettingsEditorWindow.Open(obj);
                return true;
            }
            return false;
        }
    }
}