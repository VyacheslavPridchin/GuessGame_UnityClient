using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.Events;

namespace Altterra.Navigation
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(UIAnimator))]
    public class Screen : MonoBehaviour
    {
        [field: SerializeField]
        public string Name { get; private set; } = string.Empty;

        [field: SerializeField]
        public UIAnimator Animator { get; private set; }

        [field: SerializeField]
        [field: HideInInspector]
        public UnityEvent ShowCompleteEvent { get; private set; } = new UnityEvent();

        [field: SerializeField]
        [field: HideInInspector]
        public UnityEvent HideCompleteEvent { get; private set; } = new UnityEvent();

        public RectTransform rectTransform
        {
            get
            {
                return transform as RectTransform;
            }
        }

        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                Name = gameObject.name.Replace(" ", "");
            }
        }

        public void Setup()
        {
            gameObject.SetActive(true);

            rectTransform.localPosition = Vector3.zero;
            rectTransform.localScale = Vector3.one;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.sizeDelta = Vector2.zero;

            rectTransform.SetSiblingIndex((rectTransform.parent as RectTransform).childCount);

            Animator.Setup();
        }

        public void Hide()
        {
            var callback = Animator.InvokeExitTween();
            callback.AddListener(() => gameObject.SetActive(false));
            callback.AddListener(HideCompleteEvent.Invoke);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            var callback = Animator.InvokeEntranceTween();
            callback.AddListener(ShowCompleteEvent.Invoke);
        }
    }
}