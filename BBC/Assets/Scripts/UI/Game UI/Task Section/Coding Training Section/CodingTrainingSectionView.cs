using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Scripts
{
    public class CodingTrainingSectionView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text trainingThemeLabel;
        [SerializeField]
        private GameObject trainingPagesContainer;
        [SerializeField]
        private Button previousPageButton;
        [SerializeField]
        private Button nextPageButton;
        [Space, SerializeField]
        private CodingTrainingTextPageView trainingTextPageViewPrefab;
        [SerializeField]
        private CodingTrainingTextVideoPageView trainingTextVideoPageViewPrefab;

        public void ShowTrainingTextPage(string trainingTheme, string trainingContent)
        {
            trainingThemeLabel.text = trainingTheme;

            var trainingTextPage = Instantiate(trainingTextPageViewPrefab, trainingPagesContainer.transform);
            trainingTextPage.SetContent(trainingContent);
        }

        public void ShowTrainingTextVideoPage(string trainingTheme, string trainingContent, VideoClip trainingVideo)
        {
            trainingThemeLabel.text = trainingTheme;

            var trainingTextPage = Instantiate(trainingTextVideoPageViewPrefab, trainingPagesContainer.transform);
            trainingTextPage.SetContent(trainingContent, trainingVideo);
        }
    }
}
