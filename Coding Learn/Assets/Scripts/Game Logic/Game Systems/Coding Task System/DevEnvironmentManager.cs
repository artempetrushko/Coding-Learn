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

        private PadDevEnvironmentView padDevEnvironmentView;
        private string currentTaskStartCode;
        private TaskTestData currentTaskTestInfo;

        public DevEnvironmentManager(PadDevEnvironmentView padDevEnvironmentView)
        {
            this.padDevEnvironmentView = padDevEnvironmentView;
            InitializeCompiler();
        }

        public void SetCurrentTaskInfo(string startCode, TaskTestData testInfo)
        {
            currentTaskStartCode = startCode;
            currentTaskTestInfo = testInfo;
            padDevEnvironmentView.SetDefaultCode(currentTaskStartCode);
        }

        public void ShowStartCode()
        {
            if (currentTaskStartCode != null)
            {
                padDevEnvironmentView.SetDefaultCode(currentTaskStartCode);
            } 
        }

        public void ExecuteCode()
        {
            var playerCodeStartRowNumber = currentTaskTestInfo.TestCode
                .Split("\n")
                .ToList()
                .FindIndex(line => line.Contains(currentTaskTestInfo.TestSettings.PlayerCodePlaceholder));
            var domain = ScriptDomain.CreateDomain("MyDomain", true);
            try
            {
                var testingCode = currentTaskTestInfo.TestCode.Replace(currentTaskTestInfo.TestSettings.PlayerCodePlaceholder, padDevEnvironmentView.CodeFieldContent);
                var compiledCode = domain.CompileAndLoadMainSource(testingCode);
                var proxy = compiledCode.CreateInstance();
                var isTaskCompleted = (bool)proxy.Call(currentTaskTestInfo.TestSettings.StartMethodName);
                _ = ShowExecutingProcessAsync(isTaskCompleted);
            }
            catch
            {
                Debug.LogError("There are compilation errors in runtime code!");
                var errorMessages = domain.CompileResult.Errors
                    .Select(error => (error.SourceLine - (playerCodeStartRowNumber - 1), error.SourceColumn, error.Message))
                    .ToList();
                padDevEnvironmentView.SetAndShowCompilationErrorsInfo(errorMessages);
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
                .CreateInstance(padDevEnvironmentView.gameObject)
                .Call("InitializeCompiler");
        }

        private async UniTask ShowExecutingProcessAsync(bool isTaskCompleted)
        {
            await padDevEnvironmentView.ShowExecutingProcessAsync(isTaskCompleted);
            if (isTaskCompleted)
            {
                OnTaskCompleted?.Invoke();
            }
        }
    }
}
