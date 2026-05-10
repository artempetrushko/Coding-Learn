using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using RoslynCSharp;
using TMPro;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace GameLogic
{
    public class DevEnvironmentPresenter : ITickable
    {
        public event Action TaskCompleted;
        public event Action HandbookButtonPressed;
        public event Action TipsButtonPressed;
        public event Action ChallengesButtonPressed;

        private const float VIEW_VISIBILITY_CHANGING_DURATION = 1f;
        private const int VIEW_RIGHT_MARGIN = 20;

        private DevEnvironmentConfig _devEnvironmentConfig;
        private DevEnvironmentModel _devEnvironmentModel;
        private DevEnvironmentView _devEnvironmentView;
        private RowCounterView _rowCounterViewPrefab;

        public DevEnvironmentPresenter(DevEnvironmentView devEnvironmentView, RowCounterView rowCounterViewPrefab)
        {
            _devEnvironmentView = devEnvironmentView;
            _rowCounterViewPrefab = rowCounterViewPrefab;

            _devEnvironmentView.ExecuteCodeButton.onClick.AddListener(OnExecuteCodeButtonPressed);
            _devEnvironmentView.ErrorsButton.onClick.AddListener(OnErrorsButtonPressed);
            _devEnvironmentView.ResetCodeButton.onClick.AddListener(OnResetCodeButtonPressed);
            _devEnvironmentView.HandbookButton.onClick.AddListener(OnHandbookButtonPressed);
            _devEnvironmentView.TipsButton.onClick.AddListener(OnTipsButtonPressed);
            _devEnvironmentView.ChallengesButton.onClick.AddListener(OnChallengesButtonPressed);

            InitializeCompiler();
        }

        public void Tick()
        {
            if (_devEnvironmentView.isActiveAndEnabled)
            {
                HighlightKeywords();
                UpdateRowCounters();
            }
        }

        public async UniTask ShowAsync()
        {
            _devEnvironmentView.SetActive(true);
            await SetVisibilityAsync(true);
        }

        public async UniTask HideAsync()
        {
            await SetVisibilityAsync(false);
            _devEnvironmentView.SetActive(false);
        }

        public void SetCurrentTaskInfo(DevEnvironmentModel model)
        {
            _devEnvironmentModel = model;
            SetDefaultCode();
        }

        private async UniTask SetVisibilityAsync(bool isVisible)
        {
            var movementOffsetXSign = isVisible ? 1 : -1;
            await _devEnvironmentView.transform
                .DOLocalMoveX(_devEnvironmentView.LocalPosition.x - (_devEnvironmentView.SizeDelta.x + VIEW_RIGHT_MARGIN) * movementOffsetXSign, VIEW_VISIBILITY_CHANGING_DURATION)
                .AsyncWaitForCompletion();
        }

        private void SetDefaultCode() => _devEnvironmentView.CodeFieldView.CodeInputFieldText = _devEnvironmentModel.StartCode;

        private async UniTask ExecuteCodeAsync()
        {
            var playerCodeStartRowIndex = _devEnvironmentModel.TestCode
                .Split("\n")
                .ToList()
                .FindIndex(line => line.Contains(_devEnvironmentModel.PlayerCodePlaceholder));
            var domain = ScriptDomain.CreateDomain("MyDomain", true);
            try
            {
                var testingCode = _devEnvironmentModel.TestCode.Replace(_devEnvironmentModel.PlayerCodePlaceholder, _devEnvironmentView.CodeFieldView.CodeInputFieldText);
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

        private void InitializeCompiler()
        {
            var compiledCode = ScriptDomain
                .CreateDomain("MyDomain", true)
                .CompileAndLoadMainSource(@"
using UnityEngine;
using System;

public class InitializingClass : MonoBehaviour
{
    public void InitializeCompiler() => Debug.Log(""Compiler was initialized!"");
}");
            compiledCode
                .CreateInstance()
                .Call("InitializeCompiler");
        }

        private void SetViewButtonsInteractable(bool isInteractable)
        {
            _devEnvironmentView.ExecuteCodeButton.interactable = isInteractable;
            _devEnvironmentView.ErrorsButton.interactable = isInteractable;
            _devEnvironmentView.ResetCodeButton.interactable = isInteractable;
            _devEnvironmentView.HandbookButton.interactable = isInteractable;
            _devEnvironmentView.TipsButton.interactable = isInteractable;
            _devEnvironmentView.ChallengesButton.interactable = isInteractable;
        }

        private async UniTask ShowNewErrorsAsync(string errorsMessage)
        {
            await ShowTaskCompletingIndicatorAsync(_devEnvironmentConfig.FailureColor);

            _devEnvironmentView.ErrorsSectionView.SetErrorsText(errorsMessage);
            _devEnvironmentView.ErrorsSectionView.SetScrollbarValue(1f);
            await ShowErrorsSectionAsync();
        }

        private async UniTask ShowErrorsSectionAsync()
        {
            _devEnvironmentView.ErrorsSectionView.SetActive(true);
            await SetErrorSectionVisibilityAsync(true);
        }

        private async UniTask HideErrorsSectionAsync()
        {
            await SetErrorSectionVisibilityAsync(false);
            _devEnvironmentView.ErrorsSectionView.SetActive(false);
        }

        private async UniTask SetErrorSectionVisibilityAsync(bool isVisible)
        {
            var movementSign = isVisible ? 1 : -1;
            await _devEnvironmentView.ErrorsSectionView.transform
                .DOLocalMoveY(_devEnvironmentView.ErrorsSectionView.LocalPosition.y + _devEnvironmentView.ErrorsSectionView.SizeDelta.y * movementSign, 1.5f)
                .AsyncWaitForCompletion();
        }

        private async UniTask ShowTaskSolutionCheckingAsync(bool isTaskCompleted)
        {
            _devEnvironmentView.ProgramExecutingProgressBar.color = _devEnvironmentConfig.ProgressBarNormalColor;
            await _devEnvironmentView.ProgramExecutingProgressBar.DOFillAmount(1f, _devEnvironmentConfig.ProgressBarFillingDuration);

            if (isTaskCompleted)
            {
                _devEnvironmentView.ProgramExecutingProgressBar.color = _devEnvironmentConfig.SuccessColor;
                await ShowTaskCompletingIndicatorAsync(_devEnvironmentConfig.SuccessColor);
            }

            _devEnvironmentView.ProgramExecutingProgressBar.fillAmount = 0f;
        }

        private async UniTask ShowTaskCompletingIndicatorAsync(Color indicatorColor)
        {
            _devEnvironmentView.TaskCompletingIndicator.gameObject.SetActive(true);
            _devEnvironmentView.TaskCompletingIndicator.color = indicatorColor;

            await _devEnvironmentView.TaskCompletingIndicator.DOFade(0f, 0f).AsyncWaitForCompletion();
            await _devEnvironmentView.TaskCompletingIndicator.DOFade(_devEnvironmentConfig.TaskCompletingIndicatorEndAlpha, _devEnvironmentConfig.TaskCompletingIndicatorAlphaChangingDuration).AsyncWaitForCompletion();
            await _devEnvironmentView.TaskCompletingIndicator.DOFade(0f, _devEnvironmentConfig.TaskCompletingIndicatorAlphaChangingDuration).AsyncWaitForCompletion();

            _devEnvironmentView.TaskCompletingIndicator.gameObject.SetActive(false);
        }

        private void UpdateRowCounters()
        {
            var lineInfos = _devEnvironmentView.CodeFieldView.CodeInputFieldTextInfo.lineInfo.Where(lineInfo => lineInfo.characterCount > 0).ToArray();
            var rowCountersCount = _devEnvironmentView.CodeFieldView.RowCountersContainer.transform.childCount;

            if (lineInfos.Length > rowCountersCount)
            {
                for (var i = rowCountersCount; i < lineInfos.Length; i++)
                {
                    var rowCounter = Object.Instantiate(_rowCounterViewPrefab, _devEnvironmentView.CodeFieldView.RowCountersContainer.transform);
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
                    Object.Destroy(_devEnvironmentView.CodeFieldView.RowCountersContainer.transform.GetChild(i - 1).gameObject);
                }
            }
        }

        private void HighlightKeywords()
        {
            var wordInfo = _devEnvironmentView.CodeFieldView.CodeInputFieldTextInfo.wordInfo;
            for (var i = 0; i < _devEnvironmentView.CodeFieldView.CodeInputFieldTextInfo.wordCount; i++)
            {
                var word = wordInfo[i].GetWord();
                var accordingKeywordColor = _devEnvironmentConfig.ProgrammingWordsHighlightConfig.KeywordColors.FirstOrDefault(colorWordsPair => colorWordsPair.Keywords.Contains(word));
                if (accordingKeywordColor != null)
                {
                    PaintWordBySelectedColor(wordInfo[i], accordingKeywordColor.Color);
                }
                else if (_devEnvironmentView.CodeFieldView.CodeInputFieldTextInfo.characterInfo[wordInfo[i].lastCharacterIndex + 1].character == '(')
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
                var meshIndex = _devEnvironmentView.CodeFieldView.CodeInputFieldTextInfo.characterInfo[charIndex].materialReferenceIndex;
                var vertexIndex = _devEnvironmentView.CodeFieldView.CodeInputFieldTextInfo.characterInfo[charIndex].vertexIndex;

                var vertexColors = _devEnvironmentView.CodeFieldView.CodeInputFieldTextInfo.meshInfo[meshIndex].colors32;
                for (var i = 0; i <= 3; i++)
                {
                    vertexColors[vertexIndex + i] = selectedColor;
                }
            }
            _devEnvironmentView.CodeFieldView.UpdateCodeInputFieldVertexData();
        }

        private void OnExecuteCodeButtonPressed() => ExecuteCodeAsync().Forget();

        private void OnErrorsButtonPressed()
        {
            if (_devEnvironmentView.ErrorsSectionView.isActiveAndEnabled)
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