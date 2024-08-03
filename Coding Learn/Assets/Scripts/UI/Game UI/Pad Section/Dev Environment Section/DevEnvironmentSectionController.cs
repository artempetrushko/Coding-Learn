using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Scripts
{
    public class DevEnvironmentSectionController
    {
        private DevEnvironmentSectionConfig _devEnvironmentSectionConfig;
        private DevEnvironmentSectionView _devEnvironmentSectionView;
        private CodeFieldController _codeFieldController;
        private ErrorsSectionView _errorsSectionView;

        private bool _isProcessExecuting = false;

        public DevEnvironmentSectionController(DevEnvironmentSectionView devEnvironmentSectionView)
        {
            _devEnvironmentSectionView = devEnvironmentSectionView;
        }

        public void SetDefaultCode(string defaultCode) => _codeFieldController.SetDefaultCode(defaultCode);

        public void SetAndShowCompilationErrorsInfo(List<(int line, int column, string message)> errors)
        {
            var errorsMessage = string.Join("\n", errors.Select(error => $"<color=red>Error</color> ({error.line}, {error.column}): {error.message}"));
            ShowErrorsAsync(errorsMessage).Forget();
        }

        public async UniTask ShowExecutingProcessAsync(bool isTaskCompleted)
        {
            if (!_isProcessExecuting)
            {
                _isProcessExecuting = true;

                _devEnvironmentSectionView.SetErrorsButtonInteractable(false);
                await _errorsSectionView.SetVisibilityAsync(false);
                await ShowTaskSolutionCheckingAsync(isTaskCompleted);
                if (!isTaskCompleted)
                {
                    await ShowErrorsAsync("Some of tests were failed! Try again!");
                }

                _isProcessExecuting = false;
            }     
        }

        private async UniTask ShowErrorsAsync(string errorsMessage)
        {
            await ShowTaskCompletingIndicatorAsync(false);

            _devEnvironmentSectionView.SetErrorsButtonInteractable(true);

            _errorsSectionView.SetErrorsText(errorsMessage);
            _errorsSectionView.SetScrollbarValue(1f);
            _errorsSectionView.SetVisibilityAsync(true).Forget();
        }

        public async UniTask ShowTaskSolutionCheckingAsync(bool isTaskCompleted)
        {
            _devEnvironmentSectionView.SetProgramExecutingProgressBarColor(_devEnvironmentSectionConfig.ProgressBarNormalColor);
            await _devEnvironmentSectionView.FillProgramExecutingProgressBarAsync(_devEnvironmentSectionConfig.ProgressBarFillingDuration);

            if (isTaskCompleted)
            {
                _devEnvironmentSectionView.SetProgramExecutingProgressBarColor(_devEnvironmentSectionConfig.SuccessColor);
                await ShowTaskCompletingIndicatorAsync(isTaskCompleted);
            }

            _devEnvironmentSectionView.SetProgramExecutingProgressBarFillAmount(0f);
        }

        public async UniTask ShowTaskCompletingIndicatorAsync(bool isTaskCompleted)
        {
            var indicatorColor = isTaskCompleted 
                ? _devEnvironmentSectionConfig.SuccessColor 
                : _devEnvironmentSectionConfig.FailureColor;

            _devEnvironmentSectionView.SetTaskCompletingIndicatorActive(true);
            _devEnvironmentSectionView.SetTaskCompletingIndicatorColor(indicatorColor);

            await _devEnvironmentSectionView.SetTaskCompletingIndicatorAlphaAsync(0f, 0f);
            await _devEnvironmentSectionView.SetTaskCompletingIndicatorAlphaAsync(_devEnvironmentSectionConfig.TaskCompletingIndicatorEndAlpha, _devEnvironmentSectionConfig.TaskCompletingIndicatorAlphaChangingDuration);
            await _devEnvironmentSectionView.SetTaskCompletingIndicatorAlphaAsync(0f, _devEnvironmentSectionConfig.TaskCompletingIndicatorAlphaChangingDuration);

            _devEnvironmentSectionView.SetTaskCompletingIndicatorActive(false);
        }
    }
}
