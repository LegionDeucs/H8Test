using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NoctisDev.IdentifierSystem
{
    public class IdentifierSearchProvider : ScriptableObject, ISearchWindowProvider
    {
        private List<IdentifierGroup> _groups;
        private System.Action<int, int> _onIdentifierSelected;

        public void Initialize(List<IdentifierGroup> groups, System.Action<int, int> onIdentifierSelected)
        {
            _groups = groups;
            _onIdentifierSelected = onIdentifierSelected;
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var tree = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Select Identifier"), 0)
            };
            
            for (int groupIndex = 0; groupIndex < _groups.Count; groupIndex++)
            {
                IdentifierGroup group = _groups[groupIndex];
                tree.Add(new SearchTreeGroupEntry(new GUIContent(group.Name), 1));

                for (int idIndex = 0; idIndex < group.Identifiers.Count; idIndex++)
                {
                    tree.Add(new SearchTreeEntry(new GUIContent(group.Identifiers[idIndex]))
                    {
                        level = 2,
                        userData = new IdentifierSelectionData(groupIndex, idIndex)
                    });
                }
            }

            return tree;
        }

        public bool OnSelectEntry(SearchTreeEntry entry, SearchWindowContext context)
        {
            if (entry.userData is IdentifierSelectionData data)
            {
                _onIdentifierSelected?.Invoke(data.GroupIndex, data.IdentifierIndex);
                return true;
            }

            return false;
        }
        

        private class IdentifierSelectionData
        {
            public int GroupIndex { get; }
            public int IdentifierIndex { get; }

            public IdentifierSelectionData(int groupIndex, int identifierIndex)
            {
                GroupIndex = groupIndex;
                IdentifierIndex = identifierIndex;
            }
        }
    }
}