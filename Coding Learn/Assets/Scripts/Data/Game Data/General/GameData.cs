using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    [CreateAssetMenu(fileName = "Game Data", menuName = "Game Data/Game/General/Game Data")]
    public class GameData : ScriptableObject
    {
        [SerializeField]
        private int levelsCount;

        public int LevelsCount => levelsCount;
    }
}
