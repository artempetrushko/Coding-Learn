using Cysharp.Threading.Tasks;
using RoslynCSharp;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts
{
    public class DevEnvironmentManager : MonoBehaviour
    {
        [SerializeField]
        private PadDevEnvironmentView padDevEnvironmentView;
        [Space, SerializeField]
        private UnityEvent onTaskCompleted;

        private string currentTaskStartCode;
        private TaskTestData currentTaskTestInfo;

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
            var domain = ScriptDomain.CreateDomain("MyDomain", true);
            try
            {
                var testingCode = currentTaskTestInfo.TestCode.Replace(currentTaskTestInfo.PlayerCodePlaceholder, padDevEnvironmentView.CodeFieldContent);
                var compiledCode = domain.CompileAndLoadMainSource(testingCode);
                var proxy = compiledCode.CreateInstance(gameObject);
                var isTaskCompleted = (bool)proxy.Call(currentTaskTestInfo.TestMethodName);
                _ = ShowExecutingProcessAsync(isTaskCompleted);
            }
            catch
            {
                Debug.LogError("There are compilation errors in runtime code!");
                var errorMessages = domain.CompileResult.Errors
                    .Select(error => (error.SourceLine - (currentTaskTestInfo.PlayerCodeStartRowNumber - 2), error.SourceColumn, error.Message))
                    .ToList();
                padDevEnvironmentView.SetAndShowCompilationErrorsInfo(errorMessages);
            }
        }

        private void Start()
        {
           InitializeCompiler();
        }

        private void InitializeCompiler()
        {
            var domain = ScriptDomain.CreateDomain("MyDomain", true);
            var compiledCode = domain.CompileAndLoadMainSource(@"
using UnityEngine;
using System;

public class InitializingClass : MonoBehaviour
{
    public void InitializeCompiler() => Debug.Log(""Compiler was initialized!"");
}");
            var proxy = compiledCode.CreateInstance(gameObject);
            proxy.Call("InitializeCompiler");
        }

        private async UniTask ShowExecutingProcessAsync(bool isTaskCompleted)
        {
            await padDevEnvironmentView.ShowExecutingProcessAsync(isTaskCompleted);
            if (isTaskCompleted)
            {
                onTaskCompleted.Invoke();
            }
        }
    }
}
