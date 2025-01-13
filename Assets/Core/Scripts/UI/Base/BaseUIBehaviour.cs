using UnityEngine;
using System;

namespace MyCore.UI
{
    public class BaseUIBehaviour : MonoBehaviour
    {
        [SerializeField] private bool hideOnInit;
        [SerializeField] private GameObject content;

        public event Action OnShow;
        public event Action OnHide;

        protected virtual void OnValidate()
        {

        }

        public virtual void Init()
        {
            if (hideOnInit)
            {
                HideInstant();
            }
        }

        public virtual void Show()
        {
            content.SetActive();
            OnShow?.Invoke();
        }

        public virtual void Hide()
        {
            HideInstant();
        }

        public virtual void HideInstant()
        {
            content.SetInactive();
            OnHide?.Invoke();
        }
    }
}