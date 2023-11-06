using RoslynCSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts
{
    public class PadDevEnvironmentManager : MonoBehaviour
    {
        [SerializeField]
        private PadDevEnvironmentView padDevEnvironmentView;
        [Space, SerializeField]
        private UnityEvent onTaskCompleted;

        private string currentTaskStartCode;
        private string currentTaskTestCode;

        public void SetCurrentTaskInfo(string startCode, string testCode)
        {
            currentTaskStartCode = startCode;
            currentTaskTestCode = testCode;
            padDevEnvironmentView.SetDefaultCode(startCode);
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
            ScriptDomain domain = ScriptDomain.CreateDomain("MyDomain");
            try
            {
                var robotManagementCode = currentTaskTestCode.Replace("//<playerCode>", padDevEnvironmentView.CodeFieldContent);
                ScriptType compiledCode = domain.CompileAndLoadMainSource(robotManagementCode);
                ScriptProxy proxy = compiledCode.CreateInstance(gameObject);
                StartCoroutine(ShowExecutingProcess_COR(proxy));
            }
            catch
            {
                Debug.Log("There are compilation errors in runtime code!");
                padDevEnvironmentView.SetAndShowCompilationErrorsInfo(domain.GetErrors());
            }
        }

        private void Start()
        {
            LaunchCompiler();
        }

        private void LaunchCompiler()
        {
            ScriptDomain domain = ScriptDomain.CreateDomain("MyDomain");
            ScriptType type = domain.CompileAndLoadMainSource(@"
using UnityEngine;
using System;

public class LaunchClass : MonoBehaviour
{
    public void LaunchCompiler() => Debug.Log(""Compiler is working!"");
}");
            ScriptProxy proxy = type.CreateInstance(gameObject);
            proxy.Call("LaunchCompiler");
        }

        private IEnumerator ShowExecutingProcess_COR(ScriptProxy proxy)
        {
            var isTaskCompleted = (bool)proxy.Call("isTaskCompleted");
            yield return StartCoroutine(padDevEnvironmentView.ShowExecutingProcess_COR(isTaskCompleted));
            if (isTaskCompleted)
            {
                onTaskCompleted.Invoke();
            }
        }
    }
}
