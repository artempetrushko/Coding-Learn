using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

namespace GameLogic
{
    [Serializable]
    public class LevelConfig
    {
        [SerializeField] private AssetReference _sceneReference;
        [Space]
        [SerializeField] private LocalizedString _title;
        [SerializeField] private LocalizedString _description;
        [SerializeField] private AssetReference _loadingScreenReference;
        [Space]
        [SerializeField] private LevelContent _content;

        public AssetReference SceneReference => _sceneReference;
        public LocalizedString Title => _title;
        public LocalizedString Description => _description;
        public AssetReference LoadingScreenReference => _loadingScreenReference;
        public LevelContent Content => _content;        
    }
}
