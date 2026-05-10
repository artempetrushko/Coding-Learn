using UnityEngine;

namespace GameLogic
{
    [CreateAssetMenu(fileName = "Game Config", menuName = "Game Configs/Game Config")]
    public class GameConfig : ScriptableObject
    {
        [SerializeField] private string _gameProgressSaveKey;
        [SerializeField] private LevelConfig[] _levelConfigs;

        public string GameProgressSaveKey => _gameProgressSaveKey;
        public LevelConfig[] LevelConfigs => _levelConfigs;
    }
}
