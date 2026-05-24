using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using R3;
using RoslynCSharp;
using Scripts.Core.Features.Game.CodingTasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace GameLogic
{
    public class DevEnvironmentComponent : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _codeInputField;
        [SerializeField] private GameObject _rowCountersContainer;
        [SerializeField] private Scrollbar _rowCountersContainerScrollbar;
        [SerializeField] private GameObject _errorsSection;
        [SerializeField] private TMP_Text _errorsText;
        [SerializeField] private Scrollbar _errorsTextScrollbar;
        [SerializeField] private Image _programExecutingProgressBar;
        [SerializeField] private Image _taskCompletingIndicator;
        [SerializeField] private Button _executeCodeButton;
        [SerializeField] private Button _errorsButton;
        [SerializeField] private Button _resetCodeButton;
        [Space]
        [SerializeField] private Button _handbookButton;
        [SerializeField] private Button _tipsButton;
        [SerializeField] private Button _challengesButton;

        public event Action TaskCompleted;
        public event Action HandbookButtonPressed;
        public event Action TipsButtonPressed;
        public event Action ChallengesButtonPressed;

        private const float VIEW_VISIBILITY_CHANGING_DURATION = 1f;
        private const int VIEW_RIGHT_MARGIN = 20;

        private readonly DevEnvironmentService _devEnvironmentService;
        private DevEnvironmentConfig _devEnvironmentConfig;
        private RowCounterView _rowCounterViewPrefab;

        public DevEnvironmentComponent(DevEnvironmentService devEnvironmentService, RowCounterView rowCounterViewPrefab)
        {
            _rowCounterViewPrefab = rowCounterViewPrefab;

            _executeCodeButton.onClick.AddListener(OnExecuteCodeButtonPressed);
            _errorsButton.onClick.AddListener(OnErrorsButtonPressed);
            _resetCodeButton.onClick.AddListener(OnResetCodeButtonPressed);
            _handbookButton.onClick.AddListener(OnHandbookButtonPressed);
            _tipsButton.onClick.AddListener(OnTipsButtonPressed);
            _challengesButton.onClick.AddListener(OnChallengesButtonPressed);

            _devEnvironmentService.CurrentTask.Subscribe(task =>
            {
                _codeInputField.text = task.StartCode;
            });
        }

        public void Tick()
        {
            if (isActiveAndEnabled)
            {
                HighlightKeywords();
                UpdateRowCounters();
            }
        }

        public async UniTask ShowAsync()
        {
            gameObject.SetActive(true);
            await SetVisibilityAsync(true);
        }

        public async UniTask HideAsync()
        {
            await SetVisibilityAsync(false);
            gameObject.SetActive(false);
        }

        private async UniTask SetVisibilityAsync(bool isVisible)
        {
            var movementOffsetXSign = isVisible ? 1 : -1;
            await transform
                .DOLocalMoveX(transform.localPosition.x - (GetComponent<RectTransform>().sizeDelta.x + VIEW_RIGHT_MARGIN) * movementOffsetXSign, VIEW_VISIBILITY_CHANGING_DURATION)
                .AsyncWaitForCompletion();
        }

        private void SetDefaultCode() => _codeInputField.text = _devEnvironmentModel.StartCode;

        private async UniTask ExecuteCodeAsync()
        {
            var playerCodeStartRowIndex = _devEnvironmentModel.TestCode
                .Split("\n")
                .ToList()
                .FindIndex(line => line.Contains(_devEnvironmentModel.PlayerCodePlaceholder));
            var domain = ScriptDomain.CreateDomain("MyDomain", true);
            try
            {
                var testingCode = _devEnvironmentModel.TestCode.Replace(_devEnvironmentModel.PlayerCodePlaceholder, _codeFieldView.CodeInputFieldText);
                var compiledCode = domain.CompileAndLoadMainSource(testingCode);
                var proxy = compiledCode.CreateInstance();
                var isTaskCompleted = (bool)proxy.Call(_devEnvironmentModel.TestMethodName);

                SetViewButtonsInteractable(false);
                await HideErrorsSectionAsync();
                await ShowTaskSolutionCheckingAsync(isTaskCompleted);

                if (isTaskCompleted)
                {
                    TaskCompleted?.Invoke();
                }
                else
                {
                    await ShowNewErrorsAsync(_devEnvironmentConfig.TestFailureMessage);
                }
            }
            catch
            {
                var formattedErrors = domain.CompileResult.Errors
                    .Select(error => $"<color=red>Error</color> ({error.SourceLine - playerCodeStartRowIndex}, {error.SourceColumn}): {error.Message}")
                    .ToArray();
                var errorsMessage = string.Join("\n", formattedErrors);
                ShowNewErrorsAsync(errorsMessage).Forget();
            }

            SetViewButtonsInteractable(true);
        }

        

        private void SetViewButtonsInteractable(bool isInteractable)
        {
            _executeCodeButton.interactable = isInteractable;
            _errorsButton.interactable = isInteractable;
            _resetCodeButton.interactable = isInteractable;
            _handbookButton.interactable = isInteractable;
            _tipsButton.interactable = isInteractable;
            _challengesButton.interactable = isInteractable;
        }

        private async UniTask ShowNewErrorsAsync(string errorsMessage)
        {
            await ShowTaskCompletingIndicatorAsync(_devEnvironmentConfig.FailureColor);

            _errorsText.text = errorsMessage;
            _errorsTextScrollbar.value = 1f;
            await ShowErrorsSectionAsync();
        }

        private async UniTask ShowErrorsSectionAsync()
        {
            _errorsSection.SetActive(true);
            await SetErrorSectionVisibilityAsync(true);
        }

        private async UniTask HideErrorsSectionAsync()
        {
            await SetErrorSectionVisibilityAsync(false);
            _errorsSection.SetActive(false);
        }

        private async UniTask SetErrorSectionVisibilityAsync(bool isVisible)
        {
            var movementSign = isVisible ? 1 : -1;
            await _errorsSection.transform
                .DOLocalMoveY(_errorsSection.transform.localPosition.y + _errorsSection.GetComponent<RectTransform>().sizeDelta.y * movementSign, 1.5f)
                .AsyncWaitForCompletion();
        }

        private async UniTask ShowTaskSolutionCheckingAsync(bool isTaskCompleted)
        {
            _programExecutingProgressBar.color = _devEnvironmentConfig.ProgressBarNormalColor;
            await _programExecutingProgressBar.DOFillAmount(1f, 3f);

            if (isTaskCompleted)
            {
                _programExecutingProgressBar.color = _devEnvironmentConfig.SuccessColor;
                await ShowTaskCompletingIndicatorAsync(_devEnvironmentConfig.SuccessColor);
            }

            _programExecutingProgressBar.fillAmount = 0f;
        }

        private async UniTask ShowTaskCompletingIndicatorAsync(Color indicatorColor)
        {
            _taskCompletingIndicator.gameObject.SetActive(true);
            _taskCompletingIndicator.color = indicatorColor;

            await _taskCompletingIndicator.DOFade(0f, 0f).AsyncWaitForCompletion();
            await _taskCompletingIndicator.DOFade(0.5f, 0.7f).AsyncWaitForCompletion();
            await _taskCompletingIndicator.DOFade(0f, 0.7f).AsyncWaitForCompletion();

            _taskCompletingIndicator.gameObject.SetActive(false);
        }

        private void UpdateRowCounters()
        {
            var lineInfos = _codeInputField.textComponent.textInfo.lineInfo.Where(lineInfo => lineInfo.characterCount > 0).ToArray();
            var rowCountersCount = _rowCountersContainer.transform.childCount;

            if (lineInfos.Length > rowCountersCount)
            {
                for (var i = rowCountersCount; i < lineInfos.Length; i++)
                {
                    var rowCounter = Object.Instantiate(_rowCounterViewPrefab, _rowCountersContainer.transform);
                    rowCounter.SetCounterText((i + 1).ToString());
                    rowCounter.SetCounterHeight(lineInfos[i].ascender - lineInfos[i].descender);

                    if (i + 1 < lineInfos.Length)
                    {
                        rowCounter.SetBottomGapActive(true);
                        rowCounter.SetBottomGapHeight(Mathf.Abs(lineInfos[i].descender - lineInfos[i + 1].ascender));
                    }
                }
            }
            else if (lineInfos.Length < rowCountersCount)
            {
                for (var i = rowCountersCount; i >= lineInfos.Length; i--)
                {
                    Object.Destroy(_rowCountersContainer.transform.GetChild(i - 1).gameObject);
                }
            }
        }

        private void HighlightKeywords()
        {
            var wordInfo = _codeInputField.textComponent.textInfo.wordInfo;
            for (var i = 0; i < _codeInputField.textComponent.textInfo.wordCount; i++)
            {
                var word = wordInfo[i].GetWord();
                var accordingKeywordColor = _devEnvironmentConfig.ProgrammingWordsHighlightConfig.KeywordColors.FirstOrDefault(colorWordsPair => colorWordsPair.Keywords.Contains(word));
                if (accordingKeywordColor != null)
                {
                    PaintWordBySelectedColor(wordInfo[i], accordingKeywordColor.Color);
                }
                else if (_codeInputField.textComponent.textInfo.characterInfo[wordInfo[i].lastCharacterIndex + 1].character == '(')
                {
                    PaintWordBySelectedColor(wordInfo[i], _devEnvironmentConfig.ProgrammingWordsHighlightConfig.MethodNameColor);
                }
            }
        }

        private void PaintWordBySelectedColor(TMP_WordInfo wordInfo, Color32 selectedColor)
        {
            for (var j = 0; j < wordInfo.characterCount; j++)
            {
                var charIndex = wordInfo.firstCharacterIndex + j;
                var meshIndex = _codeInputField.textComponent.textInfo.characterInfo[charIndex].materialReferenceIndex;
                var vertexIndex = _codeInputField.textComponent.textInfo.characterInfo[charIndex].vertexIndex;

                var vertexColors = _codeInputField.textComponent.textInfo.meshInfo[meshIndex].colors32;
                for (var i = 0; i <= 3; i++)
                {
                    vertexColors[vertexIndex + i] = selectedColor;
                }
            }

            _codeInputField.textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
        }

        private void OnExecuteCodeButtonPressed() => ExecuteCodeAsync().Forget();

        private void OnErrorsButtonPressed()
        {
            if (_errorsSection.activeInHierarchy)
            {
                HideErrorsSectionAsync().Forget();
            }
            else
            {
                ShowErrorsSectionAsync().Forget();
            }
        }

        private void OnResetCodeButtonPressed() => SetDefaultCode();

        private void OnHandbookButtonPressed() => HandbookButtonPressed?.Invoke();

        private void OnTipsButtonPressed() => TipsButtonPressed?.Invoke();

        private void OnChallengesButtonPressed() => ChallengesButtonPressed?.Invoke();
    }
}
