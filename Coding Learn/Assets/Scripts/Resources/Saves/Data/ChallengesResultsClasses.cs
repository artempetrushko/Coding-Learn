using System;
using System.Collections;
using System.Collections.Generic;

namespace Scripts
{
    [Serializable]
    public class LevelChallengesResults
    {
        public List<TaskChallengesResults> TasksChallengesResults = new List<TaskChallengesResults>();
    }

    [Serializable]
    public class TaskChallengesResults
    {
        public List<bool> ChallengeCompletingStatuses = new List<bool>();
    }
}
