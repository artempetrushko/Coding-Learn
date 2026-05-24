using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic
{
    public class TaskTipsСomponent : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [Space]
        [SerializeField] private TMP_Text _tipText;
        [SerializeField] private TMP_Text _tipStatusText;
        [SerializeField] private Button _showTipButton;
        [SerializeField] private Button _skipTaskButton;
        [SerializeField] private TMP_Text _skipTaskButtonText;       
        [SerializeField] private GameObject _tipFiller;

        public event Action TaskSkippingSelected;
        public event Action NewTipShown;

        private const float VISIBILITY_CHANGING_DURATION = 1.5f;

        private TaskTipsConfig _taskTipsConfig;

        private string[] _currentTaskTips;
        private int _currentTipIndex;

        public TaskTipsСomponent(TaskTipsConfig taskTipsConfig)
        {
            _taskTipsConfig = taskTipsConfig;

            _showTipButton.onClick.AddListener(OnShowTipButtonPressed);
            _skipTaskButton.onClick.AddListener(() => OnSkipTaskButtonPressedAsync().Forget());
        }

        public async UniTask ShowModalSectionAsync()
        {
            gameObject.SetActive(true);
            await _canvasGroup.DOFade(1f, VISIBILITY_CHANGING_DURATION).AsyncWaitForCompletion();
        }

        public async UniTask HideModalSectionAsync()
        {
            await _canvasGroup.DOFade(0f, VISIBILITY_CHANGING_DURATION).AsyncWaitForCompletion();
            gameObject.SetActive(false);
        }

        public void SetNewTips(string[] tips)
        {
            _currentTaskTips = tips;
            _currentTipIndex = 0;
            _tipText.text = "";
            _tipFiller.SetActive(true);

            WaitUntilNextTipAsync().Forget();
            WaitUntilTaskSkippingAsync().Forget();
        }

        private async UniTask WaitUntilNextTipAsync()
        {
            _showTipButton.interactable = false;

            var timer = TimeSpan.FromMinutes(_taskTipsConfig.NextTipTimerConfig.TimeInMinutes);
            while (timer.TotalSeconds > 0)
            {
                timer -= TimeSpan.FromSeconds(1);
                _tipStatusText.text = _taskTipsConfig.NextTipTimerConfig.ActionTimerTextsConfig.ActionWaitingText.GetLocalizedString(timer.ToString(_taskTipsConfig.TimersFormat));
                await UniTask.WaitForSeconds(1);
            }

            _tipStatusText.text = _taskTipsConfig.NextTipTimerConfig.ActionTimerTextsConfig.ActionAvailableText.GetLocalizedString();
            _showTipButton.interactable = true;
        }

        private async UniTask WaitUntilTaskSkippingAsync()
        {
            _skipTaskButton.interactable = false;

            var timer = TimeSpan.FromMinutes(_taskTipsConfig.SkipTaskTimerConfig.TimeInMinutes);
            while (timer.TotalSeconds > 0)
            {
                timer -= TimeSpan.FromSeconds(1);
                _skipTaskButtonText.text = _taskTipsConfig.SkipTaskTimerConfig.ActionTimerTextsConfig.ActionWaitingText.GetLocalizedString(timer.ToString(_taskTipsConfig.TimersFormat));
                await UniTask.WaitForSeconds(1);
            }

            _skipTaskButtonText.text = _taskTipsConfig.SkipTaskTimerConfig.ActionTimerTextsConfig.ActionAvailableText.GetLocalizedString();
            _skipTaskButton.interactable = true;
        }

        private void OnShowTipButtonPressed()
        {
            _tipFiller.SetActive(false);
            _tipText.text += _currentTaskTips[_currentTipIndex];
            _currentTipIndex++;

            if (_currentTipIndex < _currentTaskTips.Length)
            {
                WaitUntilNextTipAsync().Forget();
            }
            else
            {
                _showTipButton.interactable = false;
                _tipStatusText.text = _taskTipsConfig.NextTipTimerConfig.ActionTimerTextsConfig.ActionUnavailableText.GetLocalizedString();
            }

            NewTipShown.Invoke();
        }

        private async UniTask OnSkipTaskButtonPressedAsync()
        {
            await HideModalSectionAsync();
            TaskSkippingSelected?.Invoke();
        }
    }
}
