using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NoctisDev.IdentifierSystem
{
    public class IdentifiersEditorWindow : EditorWindow
    {
        private IdentifiersSettings _settings;
        private string _newGroupName = "New Group";
        private string _newIdentifier = "New Identifier";

        [MenuItem("Window/Identifiers Editor")]
        public static void OpenWindow()
        {
            IdentifiersEditorWindow window = GetWindow<IdentifiersEditorWindow>("Identifiers Editor");
            window.Initialize();
        }

        public void Initialize()
        {
            _settings = IdentifiersSettingsProvider.GetSettings();
            if (_settings == null)
            {
                _settings = ScriptableObject.CreateInstance<IdentifiersSettings>();
            }
        }

        private void OnGUI()
        {
            DrawAddGroupBackground();

            if (_settings.IdentifierGroups.Count == 0)
            {
                EditorGUILayout.LabelField("No Identifier Groups Found", EditorStyles.boldLabel);
            }
            else
            {
                for (int j = 0; j < _settings.IdentifierGroups.Count; j++)
                {
                    var group = _settings.IdentifierGroups[j];
                    DrawIdentifierGroupBackground(group, j);
                }
            }

            if (GUILayout.Button("Save"))
            {
                EditorUtility.SetDirty(_settings);
                AssetDatabase.SaveAssets();
            }
        }
        
        private void DrawAddGroupBackground()
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Add New Group", EditorStyles.boldLabel);
            _newGroupName = EditorGUILayout.TextField("Group Name", _newGroupName);
            if (GUILayout.Button("Add Group"))
            {
                AddNewGroup();
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawIdentifierGroupBackground(IdentifierGroup group, int index)
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField(group.Name, EditorStyles.boldLabel);
            
            for (int i = 0; i < group.Identifiers.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                group.Identifiers[i] = EditorGUILayout.TextField(group.Identifiers[i]);
                //group.Identifiers[i] = EditorGUILayout.TextField(group.Identifiers[i]);
                if (GUILayout.Button("Remove"))
                {
                    RemoveIdentifierFromGroup(group, i);
                }
                EditorGUILayout.EndHorizontal();
            }
            
            if (GUILayout.Button($"Add Identifier to {group.Name}"))
            {
                AddIdentifierToGroup(group);
            }
            
            if (GUILayout.Button($"Remove {group.Name}"))
            {
                RemoveGroup(index);
            }
            EditorGUILayout.EndVertical();
        }

        private void AddNewGroup()
        {
            if (string.IsNullOrWhiteSpace(_newGroupName)) return;

            IdentifierGroup newGroup = new IdentifierGroup { Name = _newGroupName, Identifiers = new List<string>() };
            _settings.IdentifierGroups.Add(newGroup);
            _newGroupName = "New Group";
        }

        private void AddIdentifierToGroup(IdentifierGroup group)
        {
            if (string.IsNullOrWhiteSpace(_newIdentifier)) return;

            group.Identifiers.Add(_newIdentifier);
            _newIdentifier = "New Identifier";
        }

        private void RemoveIdentifierFromGroup(IdentifierGroup group, int index)
        {
            group.Identifiers.RemoveAt(index);
        }

        private void RemoveGroup(int index)
        {
            _settings.IdentifierGroups.RemoveAt(index);
        }
    }
}