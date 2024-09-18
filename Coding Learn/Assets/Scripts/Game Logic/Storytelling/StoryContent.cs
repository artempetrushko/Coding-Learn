using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Timeline;

namespace GameLogic
{
    [CreateAssetMenu(fileName = "Story Content", menuName = "Game Configs/Story Content")]
    public class StoryContent : ScriptableObject
    {
        [SerializeField] private TimelineAsset _cutscene;
        [SerializeField] private LocalizedString[] _cutsceneScenarioParts;

        public TimelineAsset Cutscene => _cutscene;
        public LocalizedString[] CutsceneScenarioParts => _cutsceneScenarioParts;
    }
}
