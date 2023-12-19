using System;
using System.Collections;
using System.Collections.Generic;

namespace Scripts
{
    [Serializable]
    public class GameProgressData : SavedData
    {
        public int LastAvailableLevelNumber;
        public LevelChallengesResults[] AllChallengeStatuses;
    }
}
