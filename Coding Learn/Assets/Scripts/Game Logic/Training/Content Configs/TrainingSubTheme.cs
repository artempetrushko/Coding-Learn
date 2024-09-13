using UnityEngine;
using UnityEngine.Localization;

namespace GameLogic
{
    [CreateAssetMenu(fileName = "Training Sub Theme", menuName = "Game Content/Training/Training Sub Theme", order = 10)]
    public class TrainingSubTheme : ScriptableObject
    {
        [SerializeField] private LocalizedString _title;
        [SerializeField] private TrainingData[] _trainingDatas;

        public LocalizedString Title => _title;
        public TrainingData[] TrainingDatas => _trainingDatas;
    }
}
