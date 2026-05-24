using System;
using UnityEngine;
using Zenject;

namespace Core.Features.Game.Storytelling
{
    public class CutsceneSignalsHandler : MonoBehaviour
    {
        private StorytellingService _storytellingService;

        [Inject]
        public void Construct(StorytellingService storytellingService)
        {
            _storytellingService = storytellingService;
        }

        public void ShowStory() => _storytellingService.ShowNextStoryPart();

        public void StopCurrentFrame() => _storytellingService.StopCutscene();
    }
}
