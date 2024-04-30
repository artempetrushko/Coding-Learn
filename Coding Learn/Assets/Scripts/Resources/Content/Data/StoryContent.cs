using System;
using UnityEngine.Localization;
using UnityEngine.Timeline;
using UnityEngine;

namespace Scripts
{
    [Serializable]
    public class StoryContent
    {
        [field: SerializeField]
        public TimelineAsset Cutscene {  get; private set; }
        [field: SerializeField]
        public LocalizedString[] CutsceneScenarioParts { get; private set; }
    }
}
