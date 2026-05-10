using System;

namespace SaveSystem
{
	[Serializable]
    public class LevelChallengesResults
    {
        public string LevelId;
        public TaskChallengesResults[] TasksChallengesResults;
    }
}
