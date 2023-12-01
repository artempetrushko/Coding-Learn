using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts
{
    public class HandbookManager : MonoBehaviour
    {
        [SerializeField]
        private PadHandbookView padHandbookView; 
        [Space, SerializeField]
        private UnityEvent<int, int> onSubThemeButtonPressed;

        private int currentSubThemeNumber;
        private int selectedMainThemeNumber;
        private bool areMainThemeButtonsCreated = false;

        public void SetData(int currentSubThemeNumber)
        {
            this.currentSubThemeNumber = currentSubThemeNumber;
            if (areMainThemeButtonsCreated)
            {
                CreateMainThemeButtons();
                areMainThemeButtonsCreated = true;
            }
        }

        public void ShowHandbookView() => StartCoroutine(padHandbookView.ChangeVisibility_COR(true));

        public void HideHandbookView() => StartCoroutine(padHandbookView.ChangeVisibility_COR(false));

        public void ReturnToMainThemeButtons() => StartCoroutine(padHandbookView.ReturnToMainThemeButtons_COR());

        private void CreateMainThemeButtons()
        {
            var handbookMainThemes = GameContentManager.GetFirstCodingTrainingThemes(GameManager.CurrentLevelNumber)
                .Select(theme => theme.Title)
                .ToList();
            padHandbookView.CreateThemeButtons(TrainingThemeType.MainTheme, handbookMainThemes, GoToSubThemeButtons);
        }

        private void GoToSubThemeButtons(int mainThemeNumber)
        {
            selectedMainThemeNumber = mainThemeNumber;
            var subThemes = GameContentManager.GetCodingTrainingTheme(selectedMainThemeNumber).SubThemes
                .Select(info => info.Title)
                .ToList();
            if (selectedMainThemeNumber == GameManager.CurrentLevelNumber)
            {
                subThemes = subThemes.Take(currentSubThemeNumber).ToList();
            }
            padHandbookView.CreateThemeButtons(TrainingThemeType.SubTheme, subThemes, ShowSubThemeContent);
            StartCoroutine(padHandbookView.ShowSubThemeButtons_COR());
        }

        private void ShowSubThemeContent(int subThemeNumber)
        {
            onSubThemeButtonPressed.Invoke(selectedMainThemeNumber, subThemeNumber);
        }
    }
}
