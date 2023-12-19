using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class GameTaskManager : MonoBehaviour
    {
        [SerializeField]
        private CodingTrainingManager codingTrainingManager;
        [SerializeField]
        private DevEnvironmentManager devEnvironmentManager;
        [SerializeField]
        private ChallengesManager challengesManager;
        [SerializeField]
        private TipsManager tipsManager;
        [SerializeField]
        private HandbookManager handbookManager;
        [Space, SerializeField]
        private TaskSectionView taskSectionView;
        [SerializeField]
        private TaskDescriptionSectionView taskDescriptionSectionView;

        private int currentTaskNumber;
        private bool isTaskStarted;

        public void LoadNewTask()
        {
            currentTaskNumber++;
            SetTaskInfo();
            challengesManager.Initialize(currentTaskNumber);
            tipsManager.Initialize(currentTaskNumber);
            handbookManager.Initialize(currentTaskNumber);
            codingTrainingManager.ShowTrainingContent(GameManager.CurrentLevelNumber, currentTaskNumber);
        }

        public void ShowTaskContent()
        {
            if (!isTaskStarted)
            {
                isTaskStarted = true;
                challengesManager.StartChallengeTimer();
            }
            StartCoroutine(taskSectionView.ChangeMainContentVisibility_COR(true));
        }

        public void ReturnToCodingTraining(CodingTrainingInfo[] codingTrainingInfos) => StartCoroutine(ReturnToCodingTraining_COR(codingTrainingInfos));

        public void FinishTask() => StartCoroutine(ProcessTaskResults_COR(false));

        public void SkipTask() => StartCoroutine(ProcessTaskResults_COR(true));

        private IEnumerator ProcessTaskResults_COR(bool isTaskSkipped)
        {
            isTaskStarted = false;
            yield return StartCoroutine(taskSectionView.ChangeMainContentVisibility_COR(false));
            challengesManager.CheckChallengesCompleting(currentTaskNumber, isTaskSkipped);
        }

        private void SetTaskInfo()
        {
            var currentTaskInfo = GameContentManager.GetTaskInfo(currentTaskNumber);
            taskDescriptionSectionView.SetContent(currentTaskInfo.Title, currentTaskInfo.Description);
            devEnvironmentManager.SetCurrentTaskInfo(currentTaskInfo.StartCode, currentTaskInfo.TestInfo);
        }

        private IEnumerator ReturnToCodingTraining_COR(CodingTrainingInfo[] codingTrainingInfos)
        {
            yield return StartCoroutine(taskSectionView.ChangeMainContentVisibility_COR(false));
            codingTrainingManager.ShowTrainingContent(codingTrainingInfos);
        }
    }
}
