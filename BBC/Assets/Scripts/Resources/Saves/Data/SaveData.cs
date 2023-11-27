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

        public int LevelNumberToResume;
        public int LastAvailableLevelNumber;

        public List<List<List<bool>>> ChallengeCompletingStatuses;
    }
}
