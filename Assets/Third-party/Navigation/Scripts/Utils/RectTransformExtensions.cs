using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Altterra.Utils
{
    public static class RectTransformExtensions
    {
        public static Vector2 GetSizeInPixels(this RectTransform rectTransform)
        {
            Vector2 sizeInPixels = rectTransform.rect.size;

            Canvas canvas = rectTransform.GetComponentInParent<Canvas>();
            if (canvas != null)
            {
                float scaleFactor = canvas.scaleFactor;
                sizeInPixels.x *= scaleFactor;
                sizeInPixels.y *= scaleFactor;
            }

            return sizeInPixels;
        }

        public static CachedRectTransform GetCachedRectTransform(this RectTransform rectTransform)
        {
            return new CachedRectTransform(rectTransform);
        }
    }

    public struct CachedRectTransform
    {
        public CachedRectTransform(RectTransform target)
        {
            position = target.position;
            localPosition = target.localPosition;
            lossyScale = target.lossyScale;
            localScale = target.localScale;
            rotation = target.rotation;
            sizeDelta = target.sizeDelta;
            rect = target.rect;
            pivot = target.pivot;
            anchoredPosition = target.anchoredPosition;
            anchorMin = target.anchorMin;
            anchorMax = target.anchorMax;
        }

        public Vector3 position;
        public Vector3 localPosition;
        public Vector3 lossyScale;
        public Vector3 localScale;
        public Quaternion rotation;
        public Vector2 sizeDelta;
        public Rect rect;
        public Vector2 pivot;
        public Vector2 anchoredPosition;
        public Vector2 anchorMin;
        public Vector2 anchorMax;
    }
}