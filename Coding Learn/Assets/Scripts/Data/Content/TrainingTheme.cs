using UnityEngine;

namespace Scripts
{
    [CreateAssetMenu(fileName = "Training Theme", menuName = "Game Content/Training/Training Theme", order = 10)]
    public class TrainingTheme : TrainingContent
    {
        [field: SerializeField]
        public TrainingSubTheme[] SubThemes { get; private set; }
    }
}
