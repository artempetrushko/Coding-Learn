using RoslynCSharp;
using System.Collections;
using System.Collections.Generic;
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
        private TestInfo currentTaskTestInfo;

        public void SetCurrentTaskInfo(string startCode, TestInfo testInfo)
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
                StartCoroutine(ShowExecutingProcess_COR(isTaskCompleted));
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

        private IEnumerator ShowExecutingProcess_COR(bool isTaskCompleted)
        {
            yield return StartCoroutine(padDevEnvironmentView.ShowExecutingProcess_COR(isTaskCompleted));
            if (isTaskCompleted)
            {
                onTaskCompleted.Invoke();
            }
        }
    }
}
