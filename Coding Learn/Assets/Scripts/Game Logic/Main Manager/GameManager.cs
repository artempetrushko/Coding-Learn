using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private GameSaveManager saveManager;
        [SerializeField]
        private GameContentManager contentManager;
        [SerializeField]
        private StorytellingManager storytellingManager;
        [SerializeField]
        private SceneLoadingManager sceneLoadingManager;
        [Space, SerializeField]
        private GameData gameData;

        public static int CurrentLevelNumber { get; private set; }

        public void LoadNextScene()
        {
            saveManager.SaveProgress(Mathf.Clamp(CurrentLevelNumber + 1, 1, gameData.LevelsCount));

            var nextSceneIndex = (CurrentLevelNumber + 1) % (gameData.LevelsCount + 1);
            StartCoroutine(sceneLoadingManager.LoadNextSceneAsync_COR(nextSceneIndex));
        }

        private void Awake()
        {
            CurrentLevelNumber = SceneManager.GetActiveScene().buildIndex;
            saveManager.Initialize();
            saveManager.LoadSaveData();
            contentManager.LoadContentFromResources(CurrentLevelNumber);
        }

        private void Start()
        {
            storytellingManager.PlayFirstCutscene();
        }
    }
}
