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
        [Space, SerializeField]
        private TaskDescriptionSectionView taskDescriptionSectionView;
        [SerializeField]
        private TaskSectionAnimator animator;

        private int currentTaskNumber;

        public void StartNewTask()
        {
            currentTaskNumber++;
            SetTaskInfo();
            codingTrainingManager.ShowTrainingContent(GameManager.CurrentLevelNumber, currentTaskNumber);
        }

        public void ShowTaskContent() => StartCoroutine(animator.Show_COR());

        private void SetTaskInfo()
        {
            var currentTaskInfo = ContentManager.TaskInfos[currentTaskNumber - 1];
            var currentTaskTests = ContentManager.Tests[currentTaskNumber - 1];
            taskDescriptionSectionView.SetContent(currentTaskInfo.Title, currentTaskInfo.Description);
            devEnvironmentManager.SetCurrentTaskInfo(currentTaskInfo.StartCode, currentTaskTests);
        }
    }
}
