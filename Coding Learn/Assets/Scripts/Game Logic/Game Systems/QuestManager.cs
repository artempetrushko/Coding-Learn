using System;

namespace Scripts
{
    public class QuestManager : IDisposable
    {
        private StorytellingManager storytellingController;
        private TrainingManager codingTrainingController;
        private GameTaskManager gameTaskController;
        private LevelContent levelContent;
        private QuestContent currentQuest;
        private int currentQuestNumber;

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

        public QuestManager(StorytellingManager storytellingController, TrainingManager codingTrainingController, GameTaskManager gameTaskController)
        {
            this.storytellingController = storytellingController;
            this.codingTrainingController = codingTrainingController;
            this.gameTaskController = gameTaskController;

            storytellingController.CutsceneFinished += LoadCurrentQuestTraining;
            codingTrainingController.CodingTrainingDisabled += LoadCurrentQuestTask;
        }

        public void Dispose()
        {
            storytellingController.CutsceneFinished -= LoadCurrentQuestTraining;
            codingTrainingController.CodingTrainingDisabled -= LoadCurrentQuestTask;
        }

        public void LoadLevelContent(LevelContent levelContent) => this.levelContent = levelContent;

        public void StartFirstQuest()
        {
            currentQuestNumber = 1;
            StartNewQuest(levelContent.Quests[currentQuestNumber - 1]);
        }

        //public void LoadNextQuest() => LoadNewQuest(gameContentManager.GetQuest(++currentQuestNumber));

        public void StartNewQuest(QuestContent quest)
        {
            currentQuest = quest;
            storytellingController.ShowNewStoryContent(quest.Story);
        }

        public void LoadCurrentQuestTraining() => codingTrainingController.ShowTrainingContent(currentQuest.TrainingSubTheme.TrainingDatas);

        public void LoadCurrentQuestTask() => gameTaskController.LoadNewTask(currentQuest.Task);
    }
}
