using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts
{
    public class CodingTrainingManager : MonoBehaviour
    {
        [SerializeField]
        private CodingTrainingSectionView codingTrainingSectionView;
        [Space, SerializeField]
        private UnityEvent codingTrainingSectionDisabled;

        private CodingTrainingData[] currentCodingTrainingInfos;
        private int currentCodingTrainingInfoNumber;

        private int CurrentCodingTrainingInfoNumber
        {
            get => currentCodingTrainingInfoNumber;
            set
            {
                currentCodingTrainingInfoNumber = value;
                if (currentCodingTrainingInfos != null)
                {
                    ShowTrainingContentPart(currentCodingTrainingInfoNumber);
                }
            }
        }

        private TrainingShowingMode ShowingMode => CurrentCodingTrainingInfoNumber == 1 
                                                    ? TrainingShowingMode.FirstPart
                                                    : CurrentCodingTrainingInfoNumber == currentCodingTrainingInfos.Length
                                                       ? TrainingShowingMode.LastPart
                                                       : TrainingShowingMode.Normal;

        public void ShowTrainingContent(int themeNumber, int subThemeNumber) => ShowTrainingContent(GameContentManager.GetCodingTrainingInfos(themeNumber, subThemeNumber));

        public void ShowTrainingContent(CodingTrainingData[] codingTrainingInfos)
        {
            currentCodingTrainingInfos = codingTrainingInfos;
            CurrentCodingTrainingInfoNumber = 1;
            codingTrainingSectionView.Show();
        }

        public void HideTrainingContent()
        {
            UniTask.Void(async () =>
            {
                await codingTrainingSectionView.HideAsync();
                codingTrainingSectionDisabled.Invoke();
            });
        }

        public void ChangeTrainingContentPart(int offset) => CurrentCodingTrainingInfoNumber += offset;

        private void ShowTrainingContentPart(int trainingPartNumber)
        {
            /*var selectedCodingTrainingPart = currentCodingTrainingInfos[trainingPartNumber - 1];
            if (!string.IsNullOrEmpty(selectedCodingTrainingPart.VideoTitle))
            {
                codingTrainingSectionView.CreateTrainingTextVideoPage(selectedCodingTrainingPart.Title, selectedCodingTrainingPart.Info, 
                    GameContentManager.GetTrainingVideo(selectedCodingTrainingPart.VideoTitle), ShowingMode);
            }
            else
            {
                codingTrainingSectionView.CreateTrainingTextPage(selectedCodingTrainingPart.Title, selectedCodingTrainingPart.Info, ShowingMode);
            }*/
        }
    }
}
