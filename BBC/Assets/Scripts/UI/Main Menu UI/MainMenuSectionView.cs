using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
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

        public void CreateButtons(List<MainMenuButtonData> buttonDatas, Action<MainMenuSectionManager> mainMenuButtonClickedAction)
        {
            for (var i = 0; i < buttonDatas.Count; i++)
            {
                var newButton = Instantiate(mainMenuButtonPrefab, buttonsContainer.transform);
                var currentIndex = i;
                newButton.GetComponentInChildren<LocalizeStringEvent>().StringReference.SetReference("Main Menu UI", buttonDatas[currentIndex].LocalizedTextReference);
                newButton.onClick.AddListener(() => mainMenuButtonClickedAction(buttonDatas[currentIndex].LinkedSection));
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
