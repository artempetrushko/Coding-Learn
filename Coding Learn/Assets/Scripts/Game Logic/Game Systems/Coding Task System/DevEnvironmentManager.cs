using Cysharp.Threading.Tasks;
using RoslynCSharp;
using System;
using System.Linq;
using UnityEngine;

namespace Scripts
{
    public class DevEnvironmentManager
    {
        public event Action OnTaskCompleted;

        private DevEnvironmentSectionController _devEnvironmentSectionController;
        private string _currentTaskStartCode;
        private TaskTestData _currentTaskTestInfo;

        public DevEnvironmentManager(DevEnvironmentSectionController devEnvironmentSectionController)
        {
            _devEnvironmentSectionController = devEnvironmentSectionController;
            InitializeCompiler();
        }

        public void SetCurrentTaskInfo(string startCode, TaskTestData testInfo)
        {
            _currentTaskStartCode = startCode;
            _currentTaskTestInfo = testInfo;
            _devEnvironmentSectionController.SetDefaultCode(_currentTaskStartCode);
        }

        public void ShowStartCode()
        {
            if (_currentTaskStartCode != null)
            {
                _devEnvironmentSectionController.SetDefaultCode(_currentTaskStartCode);
            } 
        }

        public void ExecuteCode()
        {
            var playerCodeStartRowNumber = _currentTaskTestInfo.TestCode
                .Split("\n")
                .ToList()
                .FindIndex(line => line.Contains(_currentTaskTestInfo.TestSettings.PlayerCodePlaceholder));
            var domain = ScriptDomain.CreateDomain("MyDomain", true);
            try
            {
                var testingCode = _currentTaskTestInfo.TestCode.Replace(_currentTaskTestInfo.TestSettings.PlayerCodePlaceholder, _devEnvironmentSectionController.CodeFieldContent);
                var compiledCode = domain.CompileAndLoadMainSource(testingCode);
                var proxy = compiledCode.CreateInstance();
                var isTaskCompleted = (bool)proxy.Call(_currentTaskTestInfo.TestSettings.StartMethodName);
                ShowExecutingProcessAsync(isTaskCompleted).Forget();
            }
            catch
            {
                Debug.LogError("There are compilation errors in runtime code!");
                var errorMessages = domain.CompileResult.Errors
                    .Select(error => (error.SourceLine - (playerCodeStartRowNumber - 1), error.SourceColumn, error.Message))
                    .ToList();
                _devEnvironmentSectionController.SetAndShowCompilationErrorsInfo(errorMessages);
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
                .CreateInstance(_devEnvironmentSectionController.gameObject)
                .Call("InitializeCompiler");
        }

        private async UniTask ShowExecutingProcessAsync(bool isTaskCompleted)
        {
            await _devEnvironmentSectionController.ShowExecutingProcessAsync(isTaskCompleted);
            if (isTaskCompleted)
            {
                OnTaskCompleted?.Invoke();
            }
        }
    }
}
