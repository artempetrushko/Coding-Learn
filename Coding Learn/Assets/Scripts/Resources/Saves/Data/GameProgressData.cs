using System;

namespace Scripts
{
    [Serializable]
    public class GameProgressData : SavedData
    {
        public int LastAvailableLevelNumber;
        public LevelChallengesResults[] AllChallengeStatuses;
    }
}
