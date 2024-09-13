using UnityEngine;

namespace GameLogic
{
    [CreateAssetMenu(fileName = "Task Tips Config", menuName = "Game Data/Task Tips/Task Tips Config")]
    public class TaskTipsConfig : ScriptableObject
    {
        [SerializeField] private TipsTimerConfig _nextTipTimerConfig;
        [SerializeField] private TipsTimerConfig _skipTaskTimerConfig;
        [SerializeField] private string _timersFormat;

        public TipsTimerConfig NextTipTimerConfig => _nextTipTimerConfig;
        public TipsTimerConfig SkipTaskTimerConfig => _skipTaskTimerConfig;
        public string TimersFormat => _timersFormat;
    }
}
