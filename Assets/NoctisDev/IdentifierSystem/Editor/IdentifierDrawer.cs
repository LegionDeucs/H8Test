using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NoctisDev.IdentifierSystem
{
    [CustomPropertyDrawer(typeof(Identifier))]
    public class IdentifierDrawer : PropertyDrawer
    {
        private IdentifierSearchProvider _searchProvider;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            IdentifiersSettings settings = IdentifiersSettingsProvider.GetSettings();
            
            if (_searchProvider == null)
            {
                _searchProvider = ScriptableObject.CreateInstance<IdentifierSearchProvider>();
                if (settings != null)
                {
                    _searchProvider.Initialize(settings.IdentifierGroups, (selectedGroupIndex, selectedIdentifierIndex) =>
                    {
                        int uniqueValue = GetUniqueValue(settings.IdentifierGroups, selectedGroupIndex, selectedIdentifierIndex);
                        property.FindPropertyRelative("_value").intValue = uniqueValue;
                        property.serializedObject.ApplyModifiedProperties();
                    });
                }
            }

            SerializedProperty valueProp = property.FindPropertyRelative("_value");
            
            Rect editButtonRect = new Rect(position.x + position.width - 70, position.y, 70, position.height);
            if (GUI.Button(editButtonRect, "Edit"))
            {
                IdentifiersEditorWindow.OpenWindow();
            }
            
            if (settings == null || settings.IdentifierGroups.Count == 0)
            {
                EditorGUI.LabelField(position, label.text, "No Identifiers Found");
                return;
            }
            
            int currentValue = valueProp.intValue;
            int groupIndex = GetGroupIndex(currentValue, settings.IdentifierGroups);
            int identifierIndex = GetIdentifierIndex(currentValue, settings.IdentifierGroups, groupIndex);
            
            if (groupIndex < 0 || groupIndex >= settings.IdentifierGroups.Count)
            {
                groupIndex = 0;
            }

            IdentifierGroup currentGroup = settings.IdentifierGroups[groupIndex];
            
            if (identifierIndex < 0 || identifierIndex >= currentGroup.Identifiers.Count)
            {
                identifierIndex = currentGroup.Identifiers.Count > 0 ? 0 : -1;
            }

            string currentSetName = currentGroup.Name;
            string currentIdentifierName = identifierIndex >= 0 ? currentGroup.Identifiers[identifierIndex] : "Empty";

            string displayText = $"{currentSetName} / {currentIdentifierName}";
            
            float labelWidth = EditorStyles.label.CalcSize(label).x;
            Rect labelRect = new Rect(position.x, position.y, labelWidth, position.height);
            EditorGUI.LabelField(labelRect, label);
            
            Rect buttonRect = new Rect(position.x + labelWidth + 5, position.y, position.width - labelWidth - 75, position.height);
            if (EditorGUI.DropdownButton(buttonRect, new GUIContent(displayText), FocusType.Keyboard))
            {
                SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)), _searchProvider);
            }
        }
        
        private int GetUniqueValue(List<IdentifierGroup> groups, int groupIndex, int identifierIndex)
        {
            if (groupIndex < 0 || groupIndex >= groups.Count || identifierIndex < 0 || identifierIndex >= groups[groupIndex].Identifiers.Count)
                return -1;
            
            int uniqueValue = 0;
            for (int i = 0; i < groupIndex; i++)
            {
                uniqueValue += groups[i].Identifiers.Count;
            }
            uniqueValue += identifierIndex;
            return uniqueValue;
        }

        private int GetGroupIndex(int currentValue, List<IdentifierGroup> groups)
        {
            int identifiersCount = 0;
            
            for (int i = 0; i < groups.Count; i++)
            {
                identifiersCount += groups[i].Identifiers.Count;
                if (currentValue < identifiersCount)
                {
                    return i;
                }
            }

            return groups.Count > 0 ? groups.Count - 1 : 0;
        }

        private int GetIdentifierIndex(int currentValue, List<IdentifierGroup> groups, int groupIndex)
        {
            int identifiersCount = 0;

            for (int i = 0; i < groupIndex; i++)
            {
                identifiersCount += groups[i].Identifiers.Count;
            }
            return currentValue - identifiersCount;
        }
    }
}