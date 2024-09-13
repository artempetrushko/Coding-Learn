using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    public class TrainingView : MonoBehaviour
    {
        [SerializeField] private GameObject _content;
        [SerializeField] private TMP_Text _trainingThemeLabel;
        [SerializeField] private GameObject _trainingPagesContainer;
        [SerializeField] private Button _previousPageButton;
        [SerializeField] private Button _nextPageButton;
        [SerializeField] private Button _closeTrainingButton;
        [SerializeField] private Image[] _backgroundParts;

        public GameObject Content => _content;
        public GameObject TrainingPagesContainer => _trainingPagesContainer;
        public Button PreviousPageButton => _previousPageButton;
        public Button NextPageButton => _nextPageButton;
        public Button CloseTrainingButton => _closeTrainingButton;
        public Image[] BackgroundParts => _backgroundParts;

        public void SetTrainingThemeLabelText(string text) => _trainingThemeLabel.text = text;
    }
}
