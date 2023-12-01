using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    [Serializable]
    public class LevelChallengesResults
    {
        public TaskChallengesResults[] TasksChallengesResults;
    }

    [Serializable]
    public class TaskChallengesResults
    {
        public bool[] ChallengeCompletingStatuses;
    }
}
