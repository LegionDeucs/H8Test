using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace MyCore.Features.RatePopup
{
    public class CustomRatePopupUI : MonoBehaviour
    {
        [System.Serializable]
        public class Star
        {
            public Image icon;
            public Animator animator;
        }

        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Transform mainContent;
        [SerializeField] private Button submitButton;
        [SerializeField] private Button cancelButton;
        [SerializeField] private Button[] starButtons;

        [SerializeField] private Sprite fullStar;
        [SerializeField] private Sprite emptyStar;
        [SerializeField] private Star[] stars;

        private int rating = 0;
        private IRatePopupProvider ratingPopupProvider;


        public void Initialize(IRatePopupProvider ratingPopupProvider)
        {
            this.ratingPopupProvider = ratingPopupProvider;

            submitButton.onClick.AddListener(OnSumbitButtonClicked);
            cancelButton.onClick.AddListener(OnCancelButtonClicked);
            for (int i = 0; i < starButtons.Length; i++)
            {
                var index = i; //fix closure
                starButtons[i].onClick.AddListener(() => StarClick(index));
            }

            Show();
        }

        private void StarClick(int id)
        {
            rating = id + 1;
            for (int i = 0; i < stars.Length; i++)
            {
                if (i > id)
                {
                    stars[i].icon.sprite = emptyStar;
                    stars[i].animator.SetBool("Active", false);
                }
                else
                {
                    stars[i].icon.sprite = fullStar;
                    stars[i].animator.SetBool("Active", true);
                    stars[i].animator.Play("Shine", 0, 1 - (float)i / 5);
                }
            }
        }

        private void Show()
        {
            StarClick(5);

            canvasGroup.DOFade(1f, 0.25f).From(0f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            });
            mainContent.transform.DOScale(1f, 0.25f).From(0f).SetEase(Ease.OutBack);
        }

        private void Hide()
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.DOFade(0f, 0.25f).SetEase(Ease.InBack).From(1f).SetLink(gameObject).OnComplete(() => Destroy(gameObject));
            mainContent.transform.DOScale(0f, 0.25f).From(1f).SetEase(Ease.InBack).SetLink(gameObject);
        }

        private void OnCancelButtonClicked()
        {
            Hide();
        }

        private void OnSumbitButtonClicked()
        {
            if (rating > 3)
            {
                ratingPopupProvider.ShowRatePopup();
            }
            Hide();
        }
    }
}

