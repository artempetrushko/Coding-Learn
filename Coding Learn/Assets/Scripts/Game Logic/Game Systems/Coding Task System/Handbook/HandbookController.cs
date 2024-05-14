using System;
using System.Linq;

namespace Scripts
{
    public class HandbookController : IPadSecondaryFunction
    {
        public event Action<CodingTrainingData[]> OnSubThemeButtonPressed;

        private PadHandbookView padHandbookView;
        private TrainingTheme currentTrainingTheme;
        private TrainingSubTheme currentTrainingSubTheme;
        private bool areMainThemeButtonsCreated = false;

        public HandbookController(PadHandbookView padHandbookView)
        {
            this.padHandbookView = padHandbookView;
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

        public void ShowModalWindow() => padHandbookView.SetVisibility(true);

        public void HideModalWindow() => padHandbookView.SetVisibility(false);

        private void CreateMainThemeButtons(TrainingTheme[] trainingThemes) => padHandbookView.CreateThemeButtons(trainingThemes, GoToSubThemeButtons);

        public void ReturnToMainThemeButtons() => _ = padHandbookView.ReturnToMainThemeButtonsAsync();

        private void GoToSubThemeButtons(TrainingTheme trainingMainTheme)
        {
            int? subThemesLimit = trainingMainTheme == currentTrainingTheme
                ? trainingMainTheme.SubThemes.ToList().IndexOf(currentTrainingSubTheme) + 1
                : null;
            var subThemes = FilterHandbookTrainingSubThemes(trainingMainTheme, subThemesLimit);
            padHandbookView.CreateThemeButtons(subThemes, ShowSubThemeContent);
            _ = padHandbookView.ShowSubThemeButtonsAsync();
        }

        private void ShowSubThemeContent(TrainingSubTheme trainingSubTheme)
        {
            var codingTrainingDatas = trainingSubTheme.TrainingDatas
                .Where(trainingData => trainingData.WillAddToHandbook)
                .ToArray();
            OnSubThemeButtonPressed?.Invoke(codingTrainingDatas);
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
