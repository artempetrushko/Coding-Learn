namespace Scripts
{
    public class GameSaveController : SaveController
    {
        public TaskChallengesResults GetCurrentTaskChallengesResults(int currentTaskNumber)
        {
            var tasksChallengesResults = GameProgressData.AllChallengeStatuses[0].TasksChallengesResults; //0 is temporary
            if (currentTaskNumber > tasksChallengesResults.Count)
            {
                tasksChallengesResults.Add(new TaskChallengesResults());
            }
            return tasksChallengesResults[currentTaskNumber - 1];
        }

        public void SaveProgress(int lastAvailableNumber)
        {
            if (lastAvailableNumber > GameProgressData.LastAvailableLevelNumber)
            {
                GameProgressData.LastAvailableLevelNumber = lastAvailableNumber;
            }          
            SerializeAndSaveData(GameProgressData);
        }

        public void LoadSaveData() => GameProgressData = LoadSavedData<GameProgressData>();
    }
}
