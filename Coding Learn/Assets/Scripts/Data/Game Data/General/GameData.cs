using UnityEngine;

namespace Scripts
{
    [CreateAssetMenu(fileName = "Game Data", menuName = "Game Data/Game/General/Game Data")]
    public class GameData : ScriptableObject
    {
        [field: SerializeField]
        public LevelData[] LevelDatas { get; private set; }
    }
}
