using System;
using UnityEngine;
using UnityEngine.Localization;

namespace Scripts
{
    [CreateAssetMenu(fileName = "Level Infos", menuName = "Game Content/Main Menu Level Infos", order = 10)]
    public class MainMenuLevelsContent : ScriptableObject
    {
        [field: SerializeField]
        public MainMenuLevelInfo[] LevelInfos { get; private set; }
    }

    [Serializable]
    public class MainMenuLevelInfo
    {
        [field: SerializeField]
        public LocalizedString Title { get; private set; }
        [field: SerializeField]
        public LocalizedString Description { get; private set; }
    }
}
