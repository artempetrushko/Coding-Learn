using System;

namespace Scripts
{
    public class QuestManager : IDisposable
    {
        private StorytellingManager _storytellingManager;
        private TrainingManager _trainingManager;
        private GameTaskManager _gameTaskManager;
        private LevelContent _levelContent;
        private QuestContent _currentQuest;
        private int _currentQuestNumber;

        public QuestManager(StorytellingManager storytellingManager, TrainingManager trainingManager, GameTaskManager gameTaskManager)
        {
            _storytellingManager = storytellingManager;
            _trainingManager = trainingManager;
            _gameTaskManager = gameTaskManager;

            _storytellingManager.CutsceneFinished += LoadCurrentQuestTraining;
            _trainingManager.CodingTrainingDisabled += LoadCurrentQuestTask;
        }

        public void Dispose()
        {
            _storytellingManager.CutsceneFinished -= LoadCurrentQuestTraining;
            _trainingManager.CodingTrainingDisabled -= LoadCurrentQuestTask;
        }

        public void LoadLevelContent(LevelContent levelContent) => _levelContent = levelContent;

        public void StartFirstQuest()
        {
            _currentQuestNumber = 1;
            StartNewQuest(_levelContent.Quests[_currentQuestNumber - 1]);
        }

        public void LoadNextQuest() => LoadNewQuest(gameContentManager.GetQuest(++currentQuestNumber));

        public void StartNewQuest(QuestContent quest)
        {
            _currentQuest = quest;
            _storytellingManager.ShowNewStoryContent(quest.Story);
        }

        public void LoadCurrentQuestTraining() => _trainingManager.ShowTrainingContent(_currentQuest.TrainingSubTheme.TrainingDatas);

        public void LoadCurrentQuestTask() => _gameTaskManager.LoadNewTask(_currentQuest.Task);

        /*public void ReturnToCodingTraining(CodingTrainingInfo[] codingTrainingInfos)
        {
            UniTask.Void(async () =>
            {
                await taskSectionView.ChangeMainContentVisibilityAsync(false);
                codingTrainingManager.ShowTrainingContent(codingTrainingInfos);
            });
        }*/

        /*if (!isTaskStarted)
          {
              isTaskStarted = true;
              //challengesManager.StartChallengeTimer();
          }*/
    }
}
