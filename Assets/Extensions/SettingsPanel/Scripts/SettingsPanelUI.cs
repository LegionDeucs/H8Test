using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Collections;
using MyCore.SaveLoadSystem;
using Zenject;

namespace MyCore.Features.SettingsUI
{
    public class SettingsPanelUI : MonoBehaviour
    {
        [Header("Settings Panel")]
        [SerializeField] private CanvasGroup content;
        [SerializeField] private Transform settingsHolder;

        [Header("Toggles")]
        [SerializeField] private bool showSoundButton;
        [SerializeField] private bool showVibrationButton;
        [SerializeField] private bool showRestorePurchasesButton;
        [SerializeField] private bool showPrivacyPolicyButton;

        [Header("Buttons")]
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button soundButton;
        [SerializeField] private Button vibrationButton;
        [SerializeField] private Button closeSettingsButton;
        [SerializeField] private Button restorePurchasesButton;
        [SerializeField] private Button privacyPolicyButton;

        [Header("Images")]
        [SerializeField] private Image sound;
        [SerializeField] private Image vibration;
        [SerializeField] private Image background;

        [Header("Sprites")]
        [SerializeField] private Sprite soundOn;
        [SerializeField] private Sprite soundOff;
        [SerializeField] private Sprite vibrationOn;
        [SerializeField] private Sprite vibrationOff;

        [Header("Animation Settings")]
        [SerializeField] private float fadeDuration = 0.5f;
        [SerializeField] private float maxBackgroundFadeAlpha = 0.75f;
        [SerializeField] private TextMeshProUGUI configVersionText;

        private ISaveLoadSystem sls;

        public bool IsOpened { get; protected set; }

        [Inject]
        private void Construct(ISaveLoadSystem saveLoadSystem)
        {
            sls = saveLoadSystem;
        }

        private void OnValidate()
        {
            soundButton.gameObject.SetActive(showSoundButton);
            vibrationButton.gameObject.SetActive(showVibrationButton);
            restorePurchasesButton.gameObject.SetActive(showRestorePurchasesButton);
            privacyPolicyButton.gameObject.SetActive(showPrivacyPolicyButton);
        }

        public void Initialize()
        {
            settingsButton.onClick.AddListener(OnSettingsButtonClicked);
            soundButton.onClick.AddListener(OnSoundButtonClicked);
            vibrationButton.onClick.AddListener(OnVibrationButtonClicked);

            content.interactable = false;
            content.blocksRaycasts = false;
            content.alpha = 0f;

            closeSettingsButton.onClick.AddListener(OnCloseSettingsButtonClicked);
            restorePurchasesButton.onClick.AddListener(OnRestorePurchasesButtonClicked);
            privacyPolicyButton.onClick.AddListener(OnPrivacyPolicyButtonClicked);

#if UNITY_ANDROID && !UNITY_EDITOR
            restorePurchasesButton.SetInactive();
#endif

            StartCoroutine(SetConfigVersionText());
        }

        private void OnSettingsButtonClicked()
        {
            TogglePanel();
        }

        private void OnSoundButtonClicked()
        {
        }

        private void OnVibrationButtonClicked()
        {
        }

        private void SetSound(bool enable)
        {
            sound.sprite = enable ? soundOn : soundOff;
            AudioListener.volume = enable ? 1f : 0f;
        }

        private void SetVibration(bool enable)
        {
            vibration.sprite = enable ? vibrationOn : vibrationOff;
        }

        private IEnumerator SetConfigVersionText()
        {
            //while(!Gamestoty.initialized)
            //    yield return null;

            //configVersionText.text = Gamestoty.appVersion;

            configVersionText.text = Application.version;
            yield return null;
        }

        private void OnCloseSettingsButtonClicked()
        {
            TogglePanel();
        }

        private void OnRestorePurchasesButtonClicked()
        {
            //call restore purchases here
        }

        private void OnPrivacyPolicyButtonClicked()
        {
            //show GDPR here
        }

        private void TogglePanel()
        {
            IsOpened = !IsOpened;
            if (IsOpened)
            {
                content.interactable = true;
                content.blocksRaycasts = true;
                content.alpha = 1f;
                content.SetActive();
                background.SetActive();
                settingsHolder.transform.DOScale(1f, fadeDuration).From(0.5f).SetEase(Ease.OutBack);
                background.DOFade(maxBackgroundFadeAlpha, fadeDuration).From(0f).SetEase(Ease.OutCubic);
            }
            else
            {
                content.interactable = false;
                content.blocksRaycasts = false;
                content.SetInactive();
                background.SetInactive();
            }
        }
    }
}