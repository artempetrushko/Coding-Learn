using GameLogic;
using R3;

namespace Core.Features.Game.CodingTraining
{
    public class CodingTrainingService
    {
        private readonly GameManager _gameManager;

        public ReadOnlyReactiveProperty<TrainingData> DisplayedTrainingData;

        public CodingTrainingService(GameManager gameManager)
        {
            _gameManager = gameManager;

            _gameManager.CurrentTrainingSubTheme.Subscribe(trainingSubTheme =>
            {

            });
        }

        public void ShowNextTrainingPage()
        {

        }

        public void ShowPreviousTrainingPage()
        {

        }
    }
}
