using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using Zenject;

namespace Scripts
{
    public class GameStartup : MonoBehaviour
    {
        private GameData gameData;
        private GameSaveController saveController;
        private QuestsController questsController;
        private LevelLoadingController levelLoadingController;        

        [Inject]
        public void Construct(GameData gameData, GameSaveController saveController, QuestsController questsController, LevelLoadingController levelLoadingController)
        {
            this.gameData = gameData;
            this.saveController = saveController;
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
            saveController.LoadSaveData();

            //var currentLevel = SceneManager.GetActiveScene();           
            //var currentLevelData = gameData.LevelDatas.First(data => data.)

            questsController.LoadLevelContent(gameData.LevelDatas[0].Content);
            questsController.StartFirstQuest();
        }
    }
}
