using UnityEngine;
using UnityEngine.UI;

namespace GameLogic
{
    public class DevEnvironmentView : MonoBehaviour
    {
        [SerializeField] private CodeFieldView _codeFieldView;
        [SerializeField] private ErrorsSectionView _errorsSectionView;
        [SerializeField] private Image _programExecutingProgressBar;
        [SerializeField] private Image _taskCompletingIndicator;
        [SerializeField] private Button _executeCodeButton;
        [SerializeField] private Button _errorsButton;
        [SerializeField] private Button _resetCodeButton;
        [Space]
        [SerializeField] private Button _handbookButton;
        [SerializeField] private Button _tipsButton;
        [SerializeField] private Button _challengesButton;

        public Vector3 LocalPosition => transform.localPosition;
        public Vector2 SizeDelta => GetComponent<RectTransform>().sizeDelta;
        public CodeFieldView CodeFieldView => _codeFieldView;
        public ErrorsSectionView ErrorsSectionView => _errorsSectionView;
        public Image ProgramExecutingProgressBar => _programExecutingProgressBar;
        public Image TaskCompletingIndicator => _taskCompletingIndicator;
        public Button ExecuteCodeButton => _executeCodeButton;
        public Button ErrorsButton => _errorsButton;
        public Button ResetCodeButton => _resetCodeButton;
        public Button HandbookButton => _handbookButton;
        public Button TipsButton => _tipsButton;
        public Button ChallengesButton => _challengesButton;

        public void SetActive(bool isActive) => gameObject.SetActive(isActive);
    }
}
