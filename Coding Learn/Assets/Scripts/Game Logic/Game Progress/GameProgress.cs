using System;

namespace SaveSystem
{
    [Serializable]
    public class GameProgress
    {
        public int LastAvailableLevelNumber;
        public LevelChallengesResults[] LevelsChallengesResults;
    }
}
