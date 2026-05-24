using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using LevelLoading;
using R3;
using SaveSystem;
using Scripts.Assets.Scripts.Core.Features.Game.Storytelling;

namespace GameLogic
{
    public class GameManager : IDisposable
    {
        private GameConfig _gameConfig;

        private GameProgress _gameProgress;
        private LevelContent _levelContent;
        private QuestConfig _currentQuest;
        private int _currentQuestNumber;

        public ReadOnlyReactiveProperty<TrainingSubTheme> CurrentTrainingSubTheme => _currentQuest.TrainingSubTheme;
        public ReadOnlyReactiveProperty<StoryContent> CurrentStory;

        public GameManager(GameConfig gameConfig)
        {
            _gameConfig = gameConfig;
        }

        public void Dispose()
        {

        }

        public void StartGame()
        {
            _gameProgress = ES3.Load<GameProgress>(_gameConfig.GameProgressSaveKey);
            _levelContent = GameSessionData.CurrentLevelConfig.Content;

            _currentQuestNumber = 1;
            StartNewQuest(_levelContent.Quests[_currentQuestNumber - 1]);
        }

        private void StartNewQuest(QuestConfig quest)
        {
            _currentQuest = quest;
            _storytellingPresenter.ShowNewStoryContent(quest.Story);
        }

        public void FinishCutscene()
        {

        }

        private void FinishLevel()
        {
            var currentLevelNumber = _gameConfig.LevelConfigs.ToList().IndexOf(GameSessionData.CurrentLevelConfig) + 1;
            if (_gameProgress.LastAvailableLevelNumber < currentLevelNumber)
            {
                _gameProgress.LastAvailableLevelNumber = currentLevelNumber;
            }
            ES3.Save(_gameConfig.GameProgressSaveKey, _gameProgress);

            if (currentLevelNumber < _gameConfig.LevelConfigs.Length)
            {
                _levelLoadingPresenter.LoadLevelAsync(_gameConfig.LevelConfigs[currentLevelNumber]).Forget();
            }
            else
            {
                _levelLoadingPresenter.LoadMainMenu();
            }
        }

        private void OnCutsceneFinished() => _trainingPresenter.SetCurrentTrainingContent(_currentQuest.TrainingSubTheme.TrainingDatas);

        private void OnCodingTrainingDisabled() => _codingTaskPresenter.StartCodingTask(_currentQuest.Task);

        private void OnCodingTaskCompleted()
        {
            if (_currentQuestNumber < _levelContent.Quests.Length)
            {
                ES3.Save(_gameConfig.GameProgressSaveKey, _gameProgress);
                StartNewQuest(_levelContent.Quests[++_currentQuestNumber]);
            }
            else
            {
                FinishLevel();
            }
        }

        private void OnExitToMenuSelected() => _levelLoadingPresenter.LoadMainMenu();
    }
}
