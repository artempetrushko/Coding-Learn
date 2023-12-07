using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    [Serializable]
    public class GameProgressData : SavedData
    {
        public int LastAvailableLevelNumber;
        public LevelChallengesResults[] AllChallengeStatuses;
    }
}
