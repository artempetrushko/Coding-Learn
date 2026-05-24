using System.Linq;
using Core.Features.Game.CodingTraining.Models;
using Cysharp.Threading.Tasks;
using R3;
using RoslynCSharp;
using RoslynCSharp.Compiler;

namespace Scripts.Core.Features.Game.CodingTasks
{
    public class DevEnvironmentService
    {
        private readonly ReactiveProperty<(bool isTaskCompleted, CompilationError[] errors)> _taskCompletingStatus = new();

        private readonly CodingTaskService _codingTaskService;

        public ReadOnlyReactiveProperty<CodingTaskConfig> CurrentTask => _codingTaskService.CurrentTask;
        public ReadOnlyReactiveProperty<(bool isTaskCompleted, CompilationError[] errors)> TaskCompletingStatus => _taskCompletingStatus;

        public DevEnvironmentService(CodingTaskService codingTaskService)
        {
            _codingTaskService = codingTaskService;
        }

        public void ExecuteUserCode(string userCode)
        {
            var playerCodeStartRowIndex = CurrentTask.CurrentValue.TestCode
                .Split("\n")
                .ToList()
                .FindIndex(line => line.Contains(CurrentTask.CurrentValue.PlayerCodePlaceholder));
            var domain = ScriptDomain.CreateDomain("MyDomain", true);
            try
            {
                var testingCode = CurrentTask.CurrentValue.TestCode.Replace(CurrentTask.CurrentValue.PlayerCodePlaceholder, userCode);
                var compiledCode = domain.CompileAndLoadMainSource(testingCode);
                var proxy = compiledCode.CreateInstance();
                var isTaskCompleted = (bool)proxy.Call(CurrentTask.CurrentValue.TestMethodName);

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
    }
}
