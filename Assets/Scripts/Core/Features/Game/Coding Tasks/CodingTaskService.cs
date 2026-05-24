using System;
using System.Collections.Generic;
using Core.Features.Game.CodingTraining.Models;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using R3;

namespace Scripts.Core.Features.Game.CodingTasks
{
    public class CodingTaskService
    {
        private readonly ReactiveProperty<CodingTaskConfig> _currentTask = new();
        private readonly ReactiveProperty<bool> _isTaskFinished = new();
        private readonly ReactiveProperty<List<string>> _currentTaskTips = new(new());
        //private readonly Subject<null> AllTipsShown = new()

        private int _taskCompletingTimeInSeconds;

        public ReadOnlyReactiveProperty<CodingTaskConfig> CurrentTask => _currentTask;
        public ReadOnlyReactiveProperty<bool> IsTaskFinished => _isTaskFinished;
        public ReadOnlyReactiveProperty<List<string>> CurrentTaskTips => _currentTaskTips;

        public void StartTask(CodingTaskConfig task)
        {
            _currentTask.Value = task;

            StartTaskCompletingTimerAsync().Forget();
        }

        public void FinishCurrentTask()
        {

        }

        public void SkipCurrentTask()
        {

        }

        public void ShowNextTip()
        {

        }

        public void CheckChallengesCompleting(bool isTaskSkipped)
        {
            var challengeDatas = _currentTask.Challenges
                                    .Select(challenge => (description: challenge.Description, isCompleted: !isTaskSkipped && challenge.Checker.IsCompleted(codingTaskModel)))
                                    .ToArray();
            for (var i = 0; i < currentTaskChallengesResults.ChallengeResults.Length; i++)
            {
                if (challengeDatas[i].isCompleted && !currentTaskChallengesResults.ChallengeResults[i].IsCompleted)
                {
                    currentTaskChallengesResults.ChallengeResults[i].IsCompleted = challengeDatas[i].isCompleted;
                }
            }
        }

        private async UniTask StartTaskCompletingTimerAsync()
        {
            while (!_isTaskFinished.Value)
            {
                await UniTask.WaitForSeconds(1);
                _taskCompletingTimeInSeconds++;
            }
        }
    }
}
