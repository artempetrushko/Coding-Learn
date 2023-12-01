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

        public static int CurrentLevelNumber { get; private set; }  

        private void Awake()
        {
            CurrentLevelNumber = SceneManager.GetActiveScene().buildIndex;
            saveManager.LoadSaveData();
            contentManager.LoadContentFromResources(CurrentLevelNumber);
        }

        private void Start()
        {
            storytellingManager.PlayFirstCutscene();
        }
    }
}
