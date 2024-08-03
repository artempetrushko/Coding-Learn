using UnityEngine;
using Zenject;

namespace Scripts
{
    public class GameBootstrap : MonoBehaviour
    {
        private GameData gameData;
        private QuestManager questsController;
        private LevelLoadingManager levelLoadingController;        

        [Inject]
        public void Construct(GameData gameData, QuestManager questsController, LevelLoadingManager levelLoadingController)
        {
            this.gameData = gameData;
            this.questsController = questsController;
            this.levelLoadingController = levelLoadingController;
        }

        public void LoadNextScene()
        {
           // saveManager.SaveProgress(Mathf.Clamp(CurrentLevelNumber + 1, 1, gameData.LevelsCount));

            //var nextSceneIndex = (CurrentLevelNumber + 1) % (gameData.LevelsCount + 1);
            //_ = sceneLoadingManager.LoadNextSceneAsync(nextSceneIndex);
        }

        private void Start()
        {
            //saveController.LoadSaveData();

            //var currentLevel = SceneManager.GetActiveScene();           
            //var currentLevelData = gameData.LevelDatas.First(data => data.)

            questsController.LoadLevelContent(gameData.LevelDatas[0].Content);
            questsController.StartFirstQuest();
        }
    }
}
