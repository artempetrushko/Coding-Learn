using Cysharp.Threading.Tasks;
using System;

namespace Scripts
{
    public class TrainingManager
    {
        public event Action CodingTrainingDisabled;

        private CodingTrainingSectionView codingTrainingSectionView;
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

        public TrainingManager(CodingTrainingSectionView codingTrainingSectionView)
        {
            this.codingTrainingSectionView = codingTrainingSectionView;
        }

        public void ShowTrainingContent(CodingTrainingData[] codingTrainingDatas)
        {
            currentCodingTrainingDatas = codingTrainingDatas;
            CurrentCodingTrainingDataNumber = 1;
            codingTrainingSectionView.Show();
        }

        public void HideTrainingContent()
        {
            UniTask.Void(async () =>
            {
                await codingTrainingSectionView.HideAsync();
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
            codingTrainingSectionView.CreateTrainingPage(selectedCodingTrainingPart, trainingShowingMode);
        }
    }
}
