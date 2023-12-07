using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace Scripts
{
    public class MainMenuSectionView : MonoBehaviour
    {
        [SerializeField]
        private GameObject buttonsContainer;
        [SerializeField]
        private Button mainMenuButtonPrefab;
        [Space, SerializeField]
        private MainMenuSectionAnimator animator;

        public void CreateButtons(List<MainMenuButtonData> buttonDatas)
        {
            for (var i = 0; i < buttonDatas.Count; i++)
            {
                var newButton = Instantiate(mainMenuButtonPrefab, buttonsContainer.transform);
                var currentIndex = i;
                var buttonLocalizedString = buttonDatas[currentIndex].LocalizedString;
                newButton.GetComponentInChildren<LocalizeStringEvent>().StringReference.SetReference(buttonLocalizedString.TableReference, buttonLocalizedString.TableEntryReference);
                newButton.onClick.AddListener(buttonDatas[currentIndex].onButtonPressed.Invoke);
            }
        }

        public IEnumerator ShowContent_COR()
        {
            yield return StartCoroutine(animator.ChangeMainMenuVisibility_COR(true));
        }

        public IEnumerator HideContent_COR()
        {
            yield return StartCoroutine(animator.ChangeMainMenuVisibility_COR(false));
        }

        public IEnumerator PlayStartAnimation_COR()
        {
            animator.InitializeTweens();
            yield return StartCoroutine(animator.HideBlackScreen_COR());
            yield return StartCoroutine(animator.ChangeMainMenuVisibility_COR(true));
        }
    }
}
