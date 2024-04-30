using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

namespace Scripts
{
    [Serializable]
    public class CodingTrainingData
    {
        [field: SerializeField]
        public LocalizedString Title { get; private set; }
        [field: SerializeField]
        public LocalizedString TrainingText { get; private set; }
        [field: SerializeField]
        public AssetReference VideoGuideReference { get; private set; }
        [field: SerializeField]
        public bool WillAddToHandbook { get; private set; } = true;
    }
}
