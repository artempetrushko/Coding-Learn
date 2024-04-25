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

        public void ReturnToMainThemeButtons() => _ = padHandbookView.ReturnToMainThemeButtonsAsync();

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
            _ = padHandbookView.ShowSubThemeButtonsAsync();
        }

        private void ShowSubThemeContent(TrainingSubTheme trainingSubTheme)
        {
            var codingTrainingInfos = GameContentManager.GetHandbookCodingTrainingInfos(trainingSubTheme);
            onSubThemeButtonPressed.Invoke(codingTrainingInfos);
        }
    }
}
