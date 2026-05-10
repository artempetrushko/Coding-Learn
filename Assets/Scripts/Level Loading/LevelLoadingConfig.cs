using UnityEngine;
using UnityEngine.Localization;

namespace LevelLoading
{
    [CreateAssetMenu(fileName = "Level Loading Config", menuName = "Game Configs/Level Loading Config")]
    public class LevelLoadingConfig : ScriptableObject
    {
        [SerializeField] private LocalizedString _loadingText;

        public LocalizedString LoadingText => _loadingText;
    }
}
