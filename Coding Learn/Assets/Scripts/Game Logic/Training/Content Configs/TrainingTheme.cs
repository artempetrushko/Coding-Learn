using UnityEngine;
using UnityEngine.Localization;

namespace GameLogic
{
    [CreateAssetMenu(fileName = "Training Theme", menuName = "Game Content/Training/Training Theme", order = 10)]
    public class TrainingTheme : ScriptableObject
    {
        [SerializeField] private LocalizedString _title;
        [SerializeField] private TrainingSubTheme[] _subThemes;

        public LocalizedString Title => _title;
        public TrainingSubTheme[] SubThemes => _subThemes;
    }
}
