using System;
using UnityEngine;

namespace GameLogic
{
    [Serializable]
    public class TipsTimerConfig
    {
        [SerializeField] private int _timeInMinutes;
        [SerializeField] private ActionTimerTextsConfig _actionTimerTextsConfig;

        public int TimeInMinutes => _timeInMinutes;
        public ActionTimerTextsConfig ActionTimerTextsConfig => _actionTimerTextsConfig;
    }
}
