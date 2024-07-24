using UnityEngine;

namespace Scripts
{
    [CreateAssetMenu(fileName = "Training Sub Theme", menuName = "Game Content/Training/Training Sub Theme", order = 10)]
    public class TrainingSubTheme : TrainingContent
    {
        [field: SerializeField]
        public CodingTrainingData[] TrainingDatas { get; private set; }
    }
}
