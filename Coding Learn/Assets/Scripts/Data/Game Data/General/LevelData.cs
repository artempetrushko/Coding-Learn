using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

namespace Scripts
{
    [Serializable]
    public class LevelData
    {
        [field: SerializeField]
        public AssetReference SceneReference { get; private set; }
        [field: SerializeField, Space]
        public LocalizedString Title { get; private set; }
        [field: SerializeField]
        public LocalizedString Description { get; private set; }
        [field: SerializeField]
        public AssetReference LoadingScreenReference { get; private set; }
        [field: SerializeField, Space]
        public LevelContent Content { get; private set; }        
    }
}
