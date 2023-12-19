using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts
{
    public class HandbookManager : PadFunctionManager
    {
        [SerializeField]
        private PadHandbookView padHandbookView; 
        [Space, SerializeField]
        private UnityEvent<CodingTrainingInfo[]> onSubThemeButtonPressed;

        private int currentSubThemeNumber;
        private int selectedMainThemeNumber;
        private bool areMainThemeButtonsCreated = false;   

        public override void Initialize(int currentSubThemeNumber)
        {
            this.currentSubThemeNumber = currentSubThemeNumber;
            if (!areMainThemeButtonsCreated)
            {
                CreateMainThemeButtons();
                areMainThemeButtonsCreated = true;
            }
        }

        public override void ShowModalWindow() => padHandbookView.SetVisibility(true);

        public override void HideModalWindow() => padHandbookView.SetVisibility(false);

        public void ReturnToMainThemeButtons() => StartCoroutine(padHandbookView.ReturnToMainThemeButtons_COR());

        private void CreateMainThemeButtons()
        {
            var handbookMainThemes = GameContentManager.GetHandbookTrainingThemes(GameManager.CurrentLevelNumber);
            padHandbookView.CreateThemeButtons(handbookMainThemes, GoToSubThemeButtons);
        }

        private void GoToSubThemeButtons(TrainingMainTheme trainingMainTheme)
        {
            int? subThemesLimit = GameContentManager.GetCodingTrainingTheme(GameManager.CurrentLevelNumber).ID == trainingMainTheme.ID
                ? currentSubThemeNumber
                : null;
            var subThemes = GameContentManager.GetHandbookTrainingSubThemes(trainingMainTheme, subThemesLimit);
            padHandbookView.CreateThemeButtons(subThemes, ShowSubThemeContent);
            StartCoroutine(padHandbookView.ShowSubThemeButtons_COR());
        }

        private void ShowSubThemeContent(TrainingSubTheme trainingSubTheme)
        {
            var codingTrainingInfos = GameContentManager.GetHandbookCodingTrainingInfos(trainingSubTheme);
            onSubThemeButtonPressed.Invoke(codingTrainingInfos);
        }
    }
}
