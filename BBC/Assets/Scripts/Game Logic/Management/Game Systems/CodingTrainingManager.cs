using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

namespace Scripts
{
    public class CodingTrainingManager : MonoBehaviour
    {
        [SerializeField]
        private CodingTrainingSectionView codingTrainingSectionView;
        [Space, SerializeField]
        private string trainingVideoResourcesFolderPath = "Video/";
        [Space, SerializeField]
        private UnityEvent codingTrainingSectionDisabled;

        private CodingTrainingInfo[] currentCodingTrainingInfos;
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

        public void ShowTrainingContent(int themeNumber, int subThemeNumber)
        {
            currentCodingTrainingInfos = GameContentManager.GetCodingTrainingTheme(themeNumber).SubThemes[subThemeNumber - 1].Infos;
            CurrentCodingTrainingInfoNumber = 1;
            codingTrainingSectionView.Show();
        }

        public void HideTrainingContent() => StartCoroutine(HideTrainingContent_COR());

        public void ChangeTrainingContentPart(int offset) => CurrentCodingTrainingInfoNumber += offset;

        private void ShowTrainingContentPart(int trainingPartNumber)
        {
            var selectedCodingTrainingPart = currentCodingTrainingInfos[trainingPartNumber - 1];
            if (selectedCodingTrainingPart.VideoTitle != "")
            {
                codingTrainingSectionView.CreateTrainingTextVideoPage(selectedCodingTrainingPart.Title, selectedCodingTrainingPart.Info, Resources.Load<VideoClip>(trainingVideoResourcesFolderPath + selectedCodingTrainingPart.VideoTitle), ShowingMode);
            }
            else
            {
                codingTrainingSectionView.CreateTrainingTextPage(selectedCodingTrainingPart.Title, selectedCodingTrainingPart.Info, ShowingMode);
            }
        }

        private IEnumerator HideTrainingContent_COR()
        { 
            yield return StartCoroutine(codingTrainingSectionView.Hide_COR());
            codingTrainingSectionDisabled.Invoke();
        }
    }
}
