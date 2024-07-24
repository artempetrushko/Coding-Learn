using UnityEngine;

namespace Scripts
{
    [CreateAssetMenu(fileName = "Task Tips Config", menuName = "Game Data/Task Tips/Task Tips Config")]
    public class TaskTipsConfig : ScriptableObject
    {
        [SerializeField] private TipSectionLabelsData _tipStatusLabelsData;
        [SerializeField] private TipSectionLabelsData _taskSkippingStatusLabelsData;
        [SerializeField] private int _timeToNextTipInSeconds;
        [SerializeField] private int _timeToSkipTaskInSeconds;
    
        public TipSectionLabelsData TipStatusLabelsData => _tipStatusLabelsData;
        public TipSectionLabelsData TaskSkippingStatusLabelsData => _taskSkippingStatusLabelsData;
        public int TimeToNextTipInSeconds => _timeToNextTipInSeconds;
        public int TimeToSkipTaskInSeconds => _timeToSkipTaskInSeconds;
    }
}
