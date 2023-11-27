using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private SaveManager saveManager;
        [SerializeField]
        private GameContentManager contentManager;
        [SerializeField]
        private StorytellingManager storytellingManager;

        public static int CurrentLevelNumber { get; private set; }  

        private void Awake()
        {
            CurrentLevelNumber = SceneManager.GetActiveScene().buildIndex;
            saveManager.LoadOrCreateSaveData();
            contentManager.LoadContentFromResources(CurrentLevelNumber);
        }

        private void Start()
        {
            storytellingManager.PlayFirstCutscene();
        }
    }
}
