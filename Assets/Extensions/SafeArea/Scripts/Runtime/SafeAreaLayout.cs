using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyCore.Helpers
{
    [RequireComponent(typeof(RectTransform))]
    public class SafeAreaLayout : MonoBehaviour
    {
        private RectTransform safeContent;

        private void Awake()
        {
            safeContent = GetComponent<RectTransform>();
            ApplySafeArea();
        }

        private void ApplySafeArea()
        {
            safeContent.anchorMin = SafeArea.AnchorMin;
            safeContent.anchorMax = SafeArea.AnchorMax;
        }
    }
}
