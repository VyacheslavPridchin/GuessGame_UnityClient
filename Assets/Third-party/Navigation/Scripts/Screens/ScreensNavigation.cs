using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Altterra.Navigation
{
    public class ScreensNavigation : MonoBehaviour
    {
        [field: SerializeField]
        public List<Screen> Screens { get; private set; } = new List<Screen>();

        [SerializeField]
        [Min(-1)]
        private int defaultScreen = -1;

        [SerializeField]
        private RectTransform bodyRect;

        public Screen CurrentScreen { get; private set; }
        public Screen PreviousScreen { get => previousScreens.Last(); }

        private List<Screen> previousScreens = new List<Screen>();

        private void Start()
        {
            if (defaultScreen > -1 & defaultScreen < Screens.Count - 1)
                ShowScreenWithoutAnimation(Screens[defaultScreen].Name);
        }

        public void ShowScreenWithoutAnimation(string name)
        {
            Screen target = Screens.Find(x => x.Name == name);

            if (target == null) return;
            if (CurrentScreen != null && CurrentScreen == target) return;

            target.Animator.KillTweens(true);
            target.rectTransform.SetParent(bodyRect);
            target.Setup();
            CurrentScreen = target;
        }

        public void ShowScreen(string name)
        {
            Screen target = Screens.Find(x => x.Name == name);

            if (target == null) return;
            if (CurrentScreen != null && CurrentScreen == target) return;

            target.Animator.KillTweens(true);

            target.rectTransform.SetParent(bodyRect);

            if (CurrentScreen != null)
            {
                CurrentScreen.Animator.KillTweens(true);
                CurrentScreen.Setup();

                int currentScreenIndex = Screens.IndexOf(CurrentScreen);
                int targetScreenIndex = Screens.IndexOf(target);

                if (targetScreenIndex > currentScreenIndex)
                {
                    CurrentScreen.Animator.SetSettings(UIAnimator.Directions.Left);
                    target.Animator.SetSettings(UIAnimator.Directions.Left);
                }
                else
                {
                    CurrentScreen.Animator.SetSettings(UIAnimator.Directions.Right);
                    target.Animator.SetSettings(UIAnimator.Directions.Right);
                }

                CurrentScreen.Animator.InvokeExitTween();
                previousScreens.Add(CurrentScreen);
            }

            target.Setup();

            CurrentScreen = target;
            target.Show();
        }

        public void Backward()
        {
            if (previousScreens.Count > 0)
            {
                CurrentScreen = previousScreens.Last();
                previousScreens.RemoveAt(previousScreens.Count - 1);
            }
        }
    }
}