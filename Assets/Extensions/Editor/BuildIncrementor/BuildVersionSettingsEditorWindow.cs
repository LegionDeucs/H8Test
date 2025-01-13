using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace MyCore.BuildIncrementor
{
    public class BuildVersionSettingsEditorWindow : EditorWindow
    {
        private BuildVersionSettings buildVersionSettings;
        private SerializedObject serializedObject;

        public static void Open(BuildVersionSettings settings)
        {
            var window = GetWindow<BuildVersionSettingsEditorWindow>("Build Version Settings");
            window.minSize = new Vector2(400, 400);
            window.maxSize = new Vector2(400, 400);
            window.buildVersionSettings = settings;
            window.serializedObject = new SerializedObject(settings);
        }

        private void OnGUI()
        {
            EditorGUI.BeginChangeCheck();
            serializedObject.Update();
            DrawPlatformSettings(buildVersionSettings.Android, serializedObject.FindProperty("android"));
            DrawPlatformSettings(buildVersionSettings.IOS, serializedObject.FindProperty("iOS"));
            serializedObject.ApplyModifiedProperties();
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(buildVersionSettings);
            }
        }

        private void DrawPlatformSettings(BuildPlatformSettings platformSettings, SerializedProperty platformSettingsProperty)
        {
            EditorGUILayout.PropertyField(platformSettingsProperty, false);
            EditorGUI.indentLevel += 1;
            if (platformSettingsProperty.isExpanded)
            {
                var versionProperty = platformSettingsProperty.FindPropertyRelative("Version");
                EditorGUILayout.PropertyField(versionProperty, false);
                EditorGUI.indentLevel += 1;
                if (versionProperty.isExpanded)
                {
                    DrawNumericUpDownProperty(versionProperty.FindPropertyRelative("Major"), platformSettings.MajorVersion, r => platformSettings.MajorVersion = r);
                    DrawNumericUpDownProperty(versionProperty.FindPropertyRelative("Minor"), platformSettings.MinorVersion, r => platformSettings.MinorVersion = r);
                    DrawNumericUpDownProperty(versionProperty.FindPropertyRelative("Patch"), platformSettings.PatchVersion, r => platformSettings.PatchVersion = r);
                }
                EditorGUI.indentLevel -= 1;
                DrawSingleProperty(platformSettingsProperty.FindPropertyRelative("BuildVersion"));
                DrawNumericUpDownProperty(platformSettingsProperty.FindPropertyRelative("BuildNumber"), platformSettings.BuildNumber, r => platformSettings.BuildNumber = r);
                platformSettings.UpdateBuildVersion();
            }
            EditorGUI.indentLevel -= 1;
        }

        private void DrawNumericUpDownProperty(SerializedProperty prop, int setter, Action<int> getter)
        {
            EditorGUILayout.BeginHorizontal("box");
            EditorGUILayout.PropertyField(prop);
            if (GUILayout.Button("+", GUILayout.Width(30)))
            {
                getter.Invoke(setter + 1);
            }
            if (GUILayout.Button("-", GUILayout.Width(30)))
            {
                getter.Invoke(setter - 1);
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawSingleProperty(SerializedProperty prop)
        {
            EditorGUILayout.BeginHorizontal("box");
            EditorGUILayout.PropertyField(prop, false);
            EditorGUILayout.EndHorizontal();
        }
    }
}
