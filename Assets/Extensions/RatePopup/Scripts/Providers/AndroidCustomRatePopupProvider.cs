#if UNITY_ANDROID
using UnityEngine;

namespace TFPlay.Features.RatePopup
{
    public class AndroidCustomRatePopupProvider : IRatePopupProvider
    {
        public void ShowRatePopup()
        {
            var customPopupUIPrefabName = nameof(CustomRatePopupUI);
            var customPopupUIPrefab = Resources.Load<CustomRatePopupUI>(customPopupUIPrefabName);
            var customPopupUI = GameObject.Instantiate(customPopupUIPrefab);
            customPopupUI.name = customPopupUIPrefabName;
            GameObject.DontDestroyOnLoad(customPopupUI);
            customPopupUI.Initialize(new AndroidRatePopupProvider());
        }
    }
}
#endif