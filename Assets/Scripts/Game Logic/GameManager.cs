using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using LevelLoading;
using SaveSystem;

namespace GameLogic
{
    public class GameManager : IDisposable
    {
        private GameConfig _gameConfig;
        private LevelLoadingPresenter _levelLoadingPresenter;
        private StorytellingPresenter _storytellingPresenter;
        private TrainingPresenter _trainingPresenter;
        private CodingTaskPresenter _codingTaskPresenter;
        private ExitMenuPresenter _exitMenuPresenter;

        private GameProgress _gameProgress;
        private LevelContent _levelContent;
        private QuestConfig _currentQuest;
        private int _currentQuestNumber;

        public GameManager(GameConfig gameConfig, LevelLoadingPresenter levelLoadingPresenter, StorytellingPresenter storytellingPresenter, 
            TrainingPresenter trainingPresenter, CodingTaskPresenter codingTaskPresenter, ExitMenuPresenter exitMenuPresenter)
        {
            _gameConfig = gameConfig;
            _levelLoadingPresenter = levelLoadingPresenter;
            _storytellingPresenter = storytellingPresenter;
            _trainingPresenter = trainingPresenter;
            _codingTaskPresenter = codingTaskPresenter;
            _exitMenuPresenter = exitMenuPresenter;

            _storytellingPresenter.CutsceneFinished += OnCutsceneFinished;
            _trainingPresenter.TrainingDisabled += OnCodingTrainingDisabled;
            _codingTaskPresenter.CodingTaskCompleted += OnCodingTaskCompleted;
            _exitMenuPresenter.ExitToMenuSelected += OnExitToMenuSelected;
        }

        public void Dispose()
        {
            _storytellingPresenter.CutsceneFinished -= OnCutsceneFinished;
            _trainingPresenter.TrainingDisabled -= OnCodingTrainingDisabled;
            _codingTaskPresenter.CodingTaskCompleted -= OnCodingTaskCompleted;
            _exitMenuPresenter.ExitToMenuSelected -= OnExitToMenuSelected;
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
