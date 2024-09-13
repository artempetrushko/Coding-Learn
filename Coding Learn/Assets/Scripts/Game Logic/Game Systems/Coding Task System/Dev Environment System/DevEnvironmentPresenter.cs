using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using RoslynCSharp;
using TMPro;
using UI.Game;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace GameLogic
{
    public class DevEnvironmentPresenter : ITickable
    {
        public event Action TaskCompleted;

        private string _currentTaskStartCode;
        private TaskTestData _currentTaskTestInfo;

        private DevEnvironmentConfig _devEnvironmentConfig;
        private DevEnvironmentView _devEnvironmentView;
        private ErrorsSectionView _errorsSectionView;

        private CodeFieldView _codeFieldView;
        private RowCounterView _rowCounterPrefab;
        private ProgrammingWordsHighlightConfig _programmingWordsHighlightData;

        public DevEnvironmentPresenter(DevEnvironmentView devEnvironmentSectionView, CodeFieldView codeFieldView, RowCounterView rowCounterViewPrefab, ProgrammingWordsHighlightConfig programmingWordsHighlightData)
        {
            _devEnvironmentView = devEnvironmentSectionView;
            _codeFieldView = codeFieldView;
            _rowCounterPrefab = rowCounterViewPrefab;
            _programmingWordsHighlightData = programmingWordsHighlightData;

            _devEnvironmentView.ExecuteCodeButton.onClick.AddListener(() => ExecuteCode().Forget());

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

        }

        public void SetCurrentTaskInfo(string startCode, TaskTestData testInfo)
        {
            _currentTaskStartCode = startCode;
            _currentTaskTestInfo = testInfo;
            SetDefaultCode(_currentTaskStartCode);
        }

        public void ShowStartCode()
        {
            if (_currentTaskStartCode != null)
            {
                SetDefaultCode(_currentTaskStartCode);
            } 
        }

        public void SetDefaultCode(string code) => _codeFieldView.SetInputFieldText(code);

        private async UniTask ExecuteCode()
        {
            var playerCodeStartRowNumber = _currentTaskTestInfo.TestCode
                .Split("\n")
                .ToList()
                .FindIndex(line => line.Contains(_currentTaskTestInfo.PlayerCodePlaceholder));
            var domain = ScriptDomain.CreateDomain("MyDomain", true);
            try
            {
                var testingCode = _currentTaskTestInfo.TestCode;//.Replace(_currentTaskTestInfo.TestSettings.PlayerCodePlaceholder, _devEnvironmentSectionController.CodeFieldContent);
                var compiledCode = domain.CompileAndLoadMainSource(testingCode);
                var proxy = compiledCode.CreateInstance();
                var isTaskCompleted = (bool)proxy.Call(_currentTaskTestInfo.TestMethodName);

                _devEnvironmentView.SetErrorsButtonInteractable(false);
                await _errorsSectionView.SetVisibilityAsync(false);
                await ShowTaskSolutionCheckingAsync(isTaskCompleted);       

                if (isTaskCompleted)
                {
                    TaskCompleted?.Invoke();
                }
                else
                {
                    await ShowErrorsAsync("Some of tests were failed! Try again!");
                }
            }
            catch
            {
                var formattedErrors = domain.CompileResult.Errors
                    .Select(error => $"<color=red>Error</color> ({error.SourceLine - (playerCodeStartRowNumber - 1)}, {error.SourceColumn}): {error.Message}")
                    .ToArray();
                var errorsMessage = string.Join("\n", formattedErrors);
                ShowErrorsAsync(errorsMessage).Forget();
            }
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








        





        private async UniTask ShowErrorsAsync(string errorsMessage)
        {
            await ShowTaskCompletingIndicatorAsync(_devEnvironmentConfig.FailureColor);

            _devEnvironmentView.SetErrorsButtonInteractable(true);

            _errorsSectionView.SetErrorsText(errorsMessage);
            _errorsSectionView.SetScrollbarValue(1f);
            _errorsSectionView.SetVisibilityAsync(true).Forget();
        }

        private async UniTask ShowTaskSolutionCheckingAsync(bool isTaskCompleted)
        {
            _devEnvironmentView.SetProgramExecutingProgressBarColor(_devEnvironmentConfig.ProgressBarNormalColor);
            await _devEnvironmentView.FillProgramExecutingProgressBarAsync(_devEnvironmentConfig.ProgressBarFillingDuration);

            if (isTaskCompleted)
            {
                _devEnvironmentView.SetProgramExecutingProgressBarColor(_devEnvironmentConfig.SuccessColor);
                await ShowTaskCompletingIndicatorAsync(_devEnvironmentConfig.SuccessColor);
            }

            _devEnvironmentView.SetProgramExecutingProgressBarFillAmount(0f);
        }

        private async UniTask ShowTaskCompletingIndicatorAsync(Color indicatorColor)
        {
            _devEnvironmentView.SetTaskCompletingIndicatorActive(true);
            _devEnvironmentView.SetTaskCompletingIndicatorColor(indicatorColor);

            await _devEnvironmentView.SetTaskCompletingIndicatorAlphaAsync(0f, 0f);
            await _devEnvironmentView.SetTaskCompletingIndicatorAlphaAsync(_devEnvironmentConfig.TaskCompletingIndicatorEndAlpha, _devEnvironmentConfig.TaskCompletingIndicatorAlphaChangingDuration);
            await _devEnvironmentView.SetTaskCompletingIndicatorAlphaAsync(0f, _devEnvironmentConfig.TaskCompletingIndicatorAlphaChangingDuration);

            _devEnvironmentView.SetTaskCompletingIndicatorActive(false);
        }

        private void UpdateRowCounters()
        {
            _codeFieldView.RowCountersContainer.transform.DeleteAllChildren();

            var lineInfos = _codeFieldView.CodeInfo.lineInfo.Where(lineInfo => lineInfo.characterCount > 0).ToArray();
            for (var i = 0; i < lineInfos.Length; i++)
            {
                var rowCounter = Object.Instantiate(_rowCounterPrefab, _codeFieldView.RowCountersContainer.transform);
                rowCounter.SetCounterText((i + 1).ToString());
                rowCounter.SetCounterHeight(lineInfos[i].ascender - lineInfos[i].descender);

                if (i + 1 < lineInfos.Length)
                {
                    rowCounter.SetBottomGapActive(true);
                    rowCounter.SetBottomGapHeight(Mathf.Abs(lineInfos[i].descender - lineInfos[i + 1].ascender));
                }
            }
        }

        private void HighlightKeywords()
        {
            var wordInfo = _codeFieldView.CodeInfo.wordInfo;
            for (var i = 0; i < _codeFieldView.CodeInfo.wordCount; i++)
            {
                var word = wordInfo[i].GetWord();
                var accordingKeywordColor = _programmingWordsHighlightData.KeywordColors.FirstOrDefault(colorWordsPair => colorWordsPair.Keywords.Contains(word));
                if (accordingKeywordColor != null)
                {
                    PaintWordBySelectedColor(wordInfo[i], accordingKeywordColor.Color);
                }
                else if (_codeFieldView.CodeInfo.characterInfo[wordInfo[i].lastCharacterIndex + 1].character == '(')
                {
                    PaintWordBySelectedColor(wordInfo[i], _programmingWordsHighlightData.MethodNameColor);
                }
            }
        }

        private void PaintWordBySelectedColor(TMP_WordInfo wordInfo, Color32 selectedColor)
        {
            for (var j = 0; j < wordInfo.characterCount; j++)
            {
                var charIndex = wordInfo.firstCharacterIndex + j;
                var meshIndex = _codeFieldView.CodeInfo.characterInfo[charIndex].materialReferenceIndex;
                var vertexIndex = _codeFieldView.CodeInfo.characterInfo[charIndex].vertexIndex;

                var vertexColors = _codeFieldView.CodeInfo.meshInfo[meshIndex].colors32;
                for (var i = 0; i <= 3; i++)
                {
                    vertexColors[vertexIndex + i] = selectedColor;
                }
            }
            _codeFieldView.UpdateAllVertexesData();
        }
    }
}
