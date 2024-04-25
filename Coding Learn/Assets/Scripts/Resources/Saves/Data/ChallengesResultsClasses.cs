using System;
using System.Collections.Generic;

namespace Scripts
{
    [Serializable]
    public class LevelChallengesResults
    {
        public List<TaskChallengesResults> TasksChallengesResults = new();
    }

    [Serializable]
    public class TaskChallengesResults
    {
        public List<bool> ChallengeCompletingStatuses = new();
    }
}
