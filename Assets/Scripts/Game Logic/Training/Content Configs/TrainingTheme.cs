using UnityEngine;
using UnityEngine.Localization;

namespace GameLogic
{
    [CreateAssetMenu(fileName = "Training Theme", menuName = "Game Configs/Training/Training Theme")]
    public class TrainingTheme : ScriptableObject
    {
        [SerializeField] private LocalizedString _title;
        [SerializeField] private TrainingSubTheme[] _subThemes;

        public LocalizedString Title => _title;
        public TrainingSubTheme[] SubThemes => _subThemes;
    }
}
