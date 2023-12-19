using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Scripts
{
    public class TaskStatsView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text taskTitleText;
        [SerializeField]
        private TMP_Text starsCounterText;

        public void SetInfo(TaskStatsData data)
        {
            taskTitleText.text = data.TaskTitle;
            starsCounterText.text = $"{data.CompletedChallengesCount}/{data.TotalChallengesCount}";
        }
    }
}
