using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MyCore.Helpers
{
    public static class SafeAreaEnabler
    {
        private static SafeAreaVisualizer safeAreaVisualizer;

        public static void SetEnabled(bool enabled)
        {
            var isActive = ComponentExists(enabled);
            if (enabled && !isActive)
            {
                Show();
            }
            else if (!enabled && isActive)
            {
                Hide();
            }
        }

        private static bool ComponentExists(bool enabled)
        {
            if (safeAreaVisualizer == null && enabled)
            {
                safeAreaVisualizer = MonoBehaviour.FindObjectOfType<SafeAreaVisualizer>();
            }
            return safeAreaVisualizer != null;
        }

        private static void Show()
        {
            safeAreaVisualizer = (PrefabUtility.InstantiatePrefab(Resources.Load(nameof(SafeAreaVisualizer))) as GameObject).GetComponent<SafeAreaVisualizer>();
            safeAreaVisualizer.transform.SetSiblingIndex(int.MaxValue);
            safeAreaVisualizer.gameObject.hideFlags = HideFlags.DontSaveInBuild;
        }

        private static void Hide()
        {
            MonoBehaviour.DestroyImmediate(safeAreaVisualizer.gameObject);
        }
    }
}
