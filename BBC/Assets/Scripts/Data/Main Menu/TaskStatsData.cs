using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class TaskStatsData
    {
        public readonly string TaskTitle;
        public readonly int CompletedChallengesCount;
        public readonly int TotalChallengesCount;

        public TaskStatsData(string taskTitle, int completedChallengesCount, int totalChallengesCount)
        {
            TaskTitle = taskTitle;
            CompletedChallengesCount = completedChallengesCount;
            TotalChallengesCount = totalChallengesCount;
        }
    }
}
