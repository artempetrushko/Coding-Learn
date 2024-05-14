using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Timeline;

namespace Scripts
{
    [CreateAssetMenu(fileName = "Story Content", menuName = "Game Content/Story Content", order = 10)]
    public class StoryContent : ScriptableObject
    {
        [field: SerializeField]
        public TimelineAsset Cutscene {  get; private set; }
        [field: SerializeField]
        public LocalizedString[] CutsceneScenarioParts { get; private set; }
    }
}
