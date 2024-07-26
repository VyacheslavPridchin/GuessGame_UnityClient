using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Altterra.Utils;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.Events;

namespace Altterra.Navigation
{
    [RequireComponent(typeof(RectTransform))]
    public class UIAnimator : MonoBehaviour
    {
        [System.Serializable]
        public struct AnimationSettings
        {
            [field: SerializeField]
            public AnimationTypes Animation { get; private set; }

            [field: SerializeField]
            public Ease Ease { get; private set; }

            [field: SerializeField]
            [field: Range(0f, 5f)]
            public float Duration { get; private set; }

            [field: SerializeField]
            [field: Range(0f, 5f)]
            public float Delay { get; private set; }
        }
        public enum AnimationTypes
        {
            DelayedImmediate, Slide, Fade
        }

        public enum Directions
        {
            Left, Right
        }

        private Directions direction = Directions.Left;
        private CanvasGroup canvasGroup;
        private UnityEvent<bool> needToKillTweens = new UnityEvent<bool>();

        public RectTransform Target { get { return transform as RectTransform; } }

        [field: SerializeField]
        public AnimationSettings Entrance { get; private set; }

        [field: SerializeField]
        public AnimationSettings Exit { get; private set; }

        public void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        public UnityEvent InvokeEntranceTween()
        {
            switch (Entrance.Animation)
            {
                default:
                case AnimationTypes.Slide:
                    return InvokeEntranceSlideTween();
                case AnimationTypes.Fade:
                    return InvokeEntranceFadeTween();
                case AnimationTypes.DelayedImmediate:
                    return InvokeEntranceDelayedImmediateTween();
            }
        }

        public UnityEvent InvokeExitTween()
        {
            switch (Exit.Animation)
            {
                default:
                case AnimationTypes.Slide:
                    return InvokeExitSlideTween();
                case AnimationTypes.Fade:
                    return InvokeExitFadeTween();
                case AnimationTypes.DelayedImmediate:
                    return InvokeExitDelayedImmediateTween();
            }
        }

        #region Slide Tween

        private UnityEvent InvokeEntranceSlideTween()
        {
            CachedRectTransform target = Target.GetCachedRectTransform();
            Vector3 newPosition = target.localPosition;

            if (direction == Directions.Left)
                newPosition.x = target.rect.size.x;
            else
                newPosition.x = -target.rect.size.x;

            Target.localPosition = newPosition;
            return InvokeCoreSequence(Target.DOLocalMoveX(target.localPosition.x, Entrance.Duration), true);
        }

        private UnityEvent InvokeExitSlideTween()
        {
            Vector3 newPosition = Target.localPosition;
            if (direction == Directions.Left)
                newPosition.x = -Target.rect.size.x;
            else
                newPosition.x = Target.rect.size.x;

            return InvokeCoreSequence(Target.DOLocalMoveX(newPosition.x, Exit.Duration), false);
        }

        #endregion

        #region Fade Tween

        private UnityEvent InvokeEntranceFadeTween()
        {
            canvasGroup.alpha = 0f;
            return InvokeCoreSequence(canvasGroup.DOFade(1f, Entrance.Duration), true);
        }

        private UnityEvent InvokeExitFadeTween()
        {
            canvasGroup.alpha = 1f;
            return InvokeCoreSequence(canvasGroup.DOFade(0f, Exit.Duration), false);
        }

        #endregion

        #region Delayed Immediate Tween

        private UnityEvent InvokeEntranceDelayedImmediateTween()
        {

            canvasGroup.alpha = 0f;
            return InvokeCoreSequence(canvasGroup.DOFade(1f, 0f), true);
        }

        private UnityEvent InvokeExitDelayedImmediateTween()
        {
            canvasGroup.alpha = 1f;
            return InvokeCoreSequence(canvasGroup.DOFade(0f, 0f), false);
        }

        #endregion

        private UnityEvent InvokeCoreSequence(Tween tween, bool isEntrance)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(isEntrance ? Entrance.Delay : Exit.Delay);
            tween.SetEase(isEntrance ? Entrance.Ease : Exit.Ease);
            sequence.Append(tween);

            needToKillTweens.AddListener((bool withCallbacks) => sequence.Kill(withCallbacks));
            UnityEvent complete = new UnityEvent();
            sequence.Play().OnComplete(() => complete.Invoke());
            return complete;
        }

        public void KillTweens(bool withCallbacks)
        {
            canvasGroup.DOKill(withCallbacks);
            Target.DOKill(withCallbacks);
            needToKillTweens?.Invoke(withCallbacks);
        }

        public void SetSettings(Directions direction)
        {
            this.direction = direction;
        }

        public void Setup()
        {
            canvasGroup.alpha = 1f;
        }
    }
}