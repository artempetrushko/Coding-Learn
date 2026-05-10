using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

namespace GameLogic
{
    [Serializable]
    public class TrainingData
    {
        [SerializeField] private LocalizedString _title;
        [SerializeField] private LocalizedString _trainingText;
        [SerializeField] private AssetReference _videoGuideReference;
        [SerializeField] private bool _willAddToHandbook = true;

        public LocalizedString Title => _title;
        public LocalizedString TrainingText => _trainingText;
        public AssetReference VideoGuideReference => _videoGuideReference;
        public bool WillAddToHandbook => _willAddToHandbook;
    }
}
