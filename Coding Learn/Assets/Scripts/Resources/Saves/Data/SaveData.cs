using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    [Serializable]
    public class SaveData
    {
        public string LanguageCode;
        public int TotalLevelsCount;
        public int LastAvailableLevelNumber;
        public LevelChallengesResults[] AllChallengeStatuses;
    }
}
