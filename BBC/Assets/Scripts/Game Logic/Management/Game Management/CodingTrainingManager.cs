using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Scripts
{
    public class CodingTrainingManager : MonoBehaviour
    {
        [SerializeField]
        private CodingTrainingSectionView codingTrainingSectionView;

        public void ShowTrainingContent(int levelNumber, int taskNumber, int trainingPartNumber)
        {
            var selectedCodingTrainingInfo = ResourcesData.GetCodingTrainingInfo(levelNumber, taskNumber)[trainingPartNumber - 1];
            if (selectedCodingTrainingInfo.VideoTitles != "")
            {
                codingTrainingSectionView.ShowTrainingTextVideoPage(selectedCodingTrainingInfo.Title, selectedCodingTrainingInfo.Info, Resources.Load<VideoClip>("Video/" + selectedCodingTrainingInfo.VideoTitles));
            }
            else
            {
                codingTrainingSectionView.ShowTrainingTextPage(selectedCodingTrainingInfo.Title, selectedCodingTrainingInfo.Info);
            }
        }

        /*public void ChangeCodingTrainingPage(int coefficient)
        {
            codingTrainingPages.transform.GetChild(currentTrainingPageNumber).gameObject.SetActive(false);
            currentTrainingPageNumber += coefficient;
            codingTrainingPages.transform.GetChild(currentTrainingPageNumber).gameObject.SetActive(true);
            nextPageButton.gameObject.SetActive(currentTrainingPageNumber < codingTrainingPages.transform.childCount - 1);
            previousPageButton.gameObject.SetActive(currentTrainingPageNumber > 0);
            trainingTheme.text = selectedCodingTrainingInfo[currentTrainingPageNumber].Title;
        }*/
    }
}
