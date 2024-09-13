using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using LevelLoading;
using UI.Game;
using UnityEngine;

namespace GameLogic
{
    public class GameManager : IDisposable
    {
        private GameConfig _gameConfig;
        private LevelLoadingPresenter _levelLoadingManager;

        private StorytellingPresenter _storytellingManager;
        private TrainingPresenter _trainingManager;
        private CodingTaskPresenter _codingTaskManager;
        private LevelContent _levelContent;
        private QuestConfig _currentQuest;
        private int _currentQuestNumber;

        private ExitMenuPresenter _exitMenuPresenter;

        public GameManager(GameConfig gameConfig, LevelLoadingPresenter levelLoadingManager)
        {
            _gameConfig = gameConfig;
            _levelLoadingManager = levelLoadingManager;

            _storytellingManager.CutsceneFinished += OnCutsceneFinished;
            _trainingManager.TrainingDisabled += OnCodingTrainingDisabled;
            _codingTaskManager.CodingTaskCompleted += OnCodingTaskCompleted;
        }

        public void Dispose()
        {
            _storytellingManager.CutsceneFinished -= OnCutsceneFinished;
            _trainingManager.TrainingDisabled -= OnCodingTrainingDisabled;
            _codingTaskManager.CodingTaskCompleted -= OnCodingTaskCompleted;
        }

        public void StartGame()
        {
            //_saveManager.LoadSaveData();

            _levelContent = GameSessionData.CurrentLevelConfig.Content;

            _currentQuestNumber = 1;
            StartNewQuest(_levelContent.Quests[_currentQuestNumber - 1]);
        }

        private void StartNewQuest(QuestConfig quest)
        {
            _currentQuest = quest;
            _storytellingManager.ShowNewStoryContent(quest.Story);
        }

        private void FinishLevel()
        {
            var currentLevelNumber = _gameConfig.LevelConfigs.ToList().IndexOf(GameSessionData.CurrentLevelConfig) + 1;
            //_saveManager.SaveProgress(Mathf.Clamp(currentLevelNumber + 1, 1, _gameConfig.LevelConfigs.Length));

            if (currentLevelNumber < _gameConfig.LevelConfigs.Length)
            {
                _levelLoadingManager.LoadLevelAsync(_gameConfig.LevelConfigs[currentLevelNumber]).Forget();
            }
            else
            {
                _levelLoadingManager.LoadMainMenu();
            }
        }

        private void OnCutsceneFinished() => _trainingManager.SetCurrentTrainingContent(_currentQuest.TrainingSubTheme.TrainingDatas);

        private void OnCodingTrainingDisabled() => _codingTaskManager.StartCodingTask(_currentQuest.Task);

        private void OnCodingTaskCompleted()
        {
            if (_currentQuestNumber < _levelContent.Quests.Length)
            {
                StartNewQuest(_levelContent.Quests[++_currentQuestNumber]);
            }
            else
            {
                FinishLevel();
            }
        }

        private async UniTask ExitToMenuAsync()
        {
            //await exitToMenuSectionView.ShowBlackScreenAsync();
        }
    }
}
