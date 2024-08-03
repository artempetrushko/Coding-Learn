using Cysharp.Threading.Tasks;
using System;

namespace Scripts
{
    public class TrainingManager
    {
        public event Action CodingTrainingDisabled;

        private CodingTrainingSectionController _codingTrainingSectionController;
        private CodingTrainingData[] currentCodingTrainingDatas;
        private int currentCodingTrainingDataNumber;

        private int CurrentCodingTrainingDataNumber
        {
            get => currentCodingTrainingDataNumber;
            set
            {
                currentCodingTrainingDataNumber = value;
                if (currentCodingTrainingDatas != null)
                {
                    ShowTrainingContentPart(currentCodingTrainingDataNumber);
                }
            }
        }

        public TrainingManager(CodingTrainingSectionController codingTrainingSectionController)
        {
            _codingTrainingSectionController = codingTrainingSectionController;
        }

        public void ShowTrainingContent(CodingTrainingData[] codingTrainingDatas)
        {
            currentCodingTrainingDatas = codingTrainingDatas;
            CurrentCodingTrainingDataNumber = 1;
            _codingTrainingSectionController.Show();
        }

        public void HideTrainingContent()
        {
            UniTask.Void(async () =>
            {
                await _codingTrainingSectionController.HideAsync();
                CodingTrainingDisabled?.Invoke();
            });
        }

        public void ChangeTrainingContentPart(int offset) => CurrentCodingTrainingDataNumber += offset;

        private void ShowTrainingContentPart(int trainingPartNumber)
        {
            var selectedCodingTrainingPart = currentCodingTrainingDatas[trainingPartNumber - 1];
            var trainingShowingMode = trainingPartNumber == 1
                ? TrainingShowingMode.FirstPart
                : trainingPartNumber == currentCodingTrainingDatas.Length
                    ? TrainingShowingMode.LastPart 
                    : TrainingShowingMode.Normal;
            _codingTrainingSectionController.CreateTrainingPage(selectedCodingTrainingPart, trainingShowingMode);
        }
    }
}
