using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public enum TrainingShowingMode
    {
        Normal,
        FirstPart,
        LastPart
    }

    public class CodingTrainingSectionView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _trainingThemeLabel;
        [SerializeField] private GameObject _trainingPagesContainer;
        [SerializeField] private Button _previousPageButton;
        [SerializeField] private Button _nextPageButton;
        

        [SerializeField] private GameObject content;
        [SerializeField] private List<Image> backgroundParts;

        public GameObject TrainingPagesContainer => _trainingPagesContainer;

        public void SetTrainingThemeLabelText(string text) => _trainingThemeLabel.text = text;

        public void SetPreviousPageButtonActive(bool isActive) => _previousPageButton.gameObject.SetActive(isActive);

        public void SetNextPageButtonActive(bool isActive) => _nextPageButton.gameObject.SetActive(isActive);
    }
}
