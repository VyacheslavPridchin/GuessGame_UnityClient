using System.Collections.Generic;
using UnityEngine;
using Altterra.Utils;

namespace Altterra.Navigation
{
    public class PopupsManager : Singleton<PopupsManager>
    {
        [field: SerializeField]
        public List<Popup> Popups { get; private set; } = new List<Popup>();

        [SerializeField]
        private RectTransform bodyRect;

        private void Start()
        {
            for (int i = 0; i < Popups.Count; i++)
            {
                Popups[i].gameObject.SetActive(false);
            }
        }

        public Popup ShowPopupWithoutAnimation(string name)
        {
            Popup target = Popups.Find(x => x.Name == name);

            if (target == null) return null;

            target.Animator.KillTweens(true);
            target.rectTransform.SetParent(bodyRect);
            target.Setup();

            return target;

        }

        public Popup ShowPopup(string name)
        {
            Popup target = Popups.Find(x => x.Name == name);

            if (target == null) return null;

            target.Animator.KillTweens(true);

            target.rectTransform.SetParent(bodyRect);

            target.Setup();
            target.Show();

            return target;
        }

        public Popup GetPopup(string name)
        {
            Popup target = Popups.Find(x => x.Name == name);
            return target;
        }
    }
}