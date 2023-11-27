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
        private HandbookManager handbookManager;
        [Space, SerializeField]
        private TaskSectionView taskSectionView;
        [SerializeField]
        private TaskDescriptionSectionView taskDescriptionSectionView;

        private int currentTaskNumber;

        public void StartNewTask()
        {
            currentTaskNumber++;
            SetTaskInfo();
            handbookManager.SetData(currentTaskNumber);
            codingTrainingManager.ShowTrainingContent(GameManager.CurrentLevelNumber, currentTaskNumber);
        }

        public void ShowTaskContent() => StartCoroutine(taskSectionView.ChangeMainContentVisibility_COR(true));

        private void SetTaskInfo()
        {
            var currentTaskInfo = GameContentManager.GetTaskInfo(currentTaskNumber);
            var currentTaskTests = GameContentManager.GetTests(currentTaskNumber);
            taskDescriptionSectionView.SetContent(currentTaskInfo.Title, currentTaskInfo.Description);
            devEnvironmentManager.SetCurrentTaskInfo(currentTaskInfo.StartCode, currentTaskTests);
        }
    }
}
