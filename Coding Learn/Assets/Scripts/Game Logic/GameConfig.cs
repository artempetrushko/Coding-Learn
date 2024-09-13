using UnityEngine;

namespace GameLogic
{
    [CreateAssetMenu(fileName = "Game Data", menuName = "Game Data/Game/General/Game Config")]
    public class GameConfig : ScriptableObject
    {
        [SerializeField] private string _gameProgressSaveKey;
        [SerializeField] private LevelConfig[] _levelConfigs;

        public string GameProgressSaveKey => _gameProgressSaveKey;
        public LevelConfig[] LevelConfigs => _levelConfigs;
    }
}
