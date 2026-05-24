using System;
using GameLogic;
using R3;
using UnityEngine.Playables;

namespace Core.Features.Game.Storytelling
{
    public class StorytellingService : IDisposable
    {
        private readonly GameManager _gameManager;
        private readonly PlayableDirector _playableDirector;

        private int _currentStoryPartArticleNumber;

        public StorytellingService(GameManager gameManager, PlayableDirector playableDirector)
        {
            _gameManager = gameManager;
            _playableDirector = playableDirector;

            _playableDirector.stopped += OnPlayableDirectorStopped;

            _gameManager.CurrentStory.Subscribe(story =>
            {
                _currentStoryPartArticleNumber = 1;

                _playableDirector.time = 0;
                _playableDirector.Play(story.Cutscene);
            });
        }

        public void Dispose()
        {
            _playableDirector.stopped -= OnPlayableDirectorStopped;
        }

        public void ShowNextStoryPart()
        {

        }

        public void StopCutscene()
        {

        }

        private void OnPlayableDirectorStopped(PlayableDirector playableDirector)
        {
            if (playableDirector.time >= _playableDirector.playableAsset.duration)
            {
                _gameManager.FinishCutscene();
            }
        }
    }
}
