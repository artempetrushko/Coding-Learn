using System;
using System.Linq;

namespace Scripts
{
    public class HandbookManager : IPadSecondaryFunction
    {
        public event Action<CodingTrainingData[]> SubThemeButtonPressed;

        private HandbookSectionController _handbookSectionController;
        private TrainingTheme currentTrainingTheme;
        private TrainingSubTheme currentTrainingSubTheme;
        private bool areMainThemeButtonsCreated = false;

        public HandbookManager(HandbookSectionController handbookSectionController)
        {
            _handbookSectionController = handbookSectionController;
        }

        public void SetNewTrainingContent(TrainingTheme currentTrainingTheme, TrainingSubTheme currentTrainingSubTheme)
        {
            this.currentTrainingTheme = currentTrainingTheme;
            this.currentTrainingSubTheme = currentTrainingSubTheme;
            if (!areMainThemeButtonsCreated)
            {
                //CreateMainThemeButtons();
                areMainThemeButtonsCreated = true;
            }
        }

        public void ShowModalSection() => _handbookSectionController.SetVisibility(true);

        public void HideModalSection() => _handbookSectionController.SetVisibility(false);

        private void CreateMainThemeButtons(TrainingTheme[] trainingThemes) => _handbookSectionController.CreateThemeButtons(trainingThemes, GoToSubThemeButtons);

        public void ReturnToMainThemeButtons() => _ = _handbookSectionController.ReturnToMainThemeButtonsAsync();

        private void GoToSubThemeButtons(TrainingTheme trainingMainTheme)
        {
            int? subThemesLimit = trainingMainTheme == currentTrainingTheme
                ? trainingMainTheme.SubThemes.ToList().IndexOf(currentTrainingSubTheme) + 1
                : null;
            var subThemes = FilterHandbookTrainingSubThemes(trainingMainTheme, subThemesLimit);
            _handbookSectionController.CreateThemeButtons(subThemes, ShowSubThemeContent);
            _ = _handbookSectionController.ShowSubThemeButtonsAsync();
        }

        private void ShowSubThemeContent(TrainingSubTheme trainingSubTheme)
        {
            var codingTrainingDatas = trainingSubTheme.TrainingDatas
                .Where(trainingData => trainingData.WillAddToHandbook)
                .ToArray();
            SubThemeButtonPressed?.Invoke(codingTrainingDatas);
        }

        private TrainingSubTheme[] FilterHandbookTrainingSubThemes(TrainingTheme mainTheme, int? lastAvailableSubThemeNumber = null)
        {
            var subThemes = mainTheme.SubThemes;
            if (lastAvailableSubThemeNumber != null)
            {
                subThemes = subThemes.Take(lastAvailableSubThemeNumber.Value).ToArray();
            }
            return subThemes
                .Where(subTheme => subTheme.TrainingDatas.Any(data => data.WillAddToHandbook))
                .ToArray();
        }
    }
}
