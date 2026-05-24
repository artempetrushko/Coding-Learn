using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Timeline;

namespace Scripts.Assets.Scripts.Core.Features.Game.Storytelling
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
