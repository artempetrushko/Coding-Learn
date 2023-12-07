using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField]
        private MainMenuSectionView mainMenuSectionView;
        [SerializeField]
        private List<MainMenuButtonData> buttonDatas = new List<MainMenuButtonData>();
        [Space, SerializeField]
        private LevelsManager levelsManager;
        [SerializeField]
        private StatsManager statsManager;
        [SerializeField]
        private SettingsManager settingsManager;
        [SerializeField]
        private MainMenuSaveManager saveManager;
        [SerializeField]
        private MainMenuContentManager contentManager;
        [Space, SerializeField]
        private GameData gameData;

        private MainMenuSectionManager currentMainMenuSection;

        public void ShowSelectedSection(MainMenuSectionManager section) => StartCoroutine(ShowSelectedSection_COR(section));

        public void HideCurrentSection() => StartCoroutine(HideCurrentSection_COR());

        public IEnumerator ShowSelectedSection_COR(MainMenuSectionManager section)
        {
            currentMainMenuSection = section;
            yield return StartCoroutine(mainMenuSectionView.HideContent_COR());
            yield return StartCoroutine(section.ShowSectionView_COR());
        }

        public IEnumerator HideCurrentSection_COR()
        {
            yield return StartCoroutine(currentMainMenuSection.HideSectionView_COR());
            yield return StartCoroutine(mainMenuSectionView.ShowContent_COR());
            currentMainMenuSection = null;
        }

        public void Exit() => Application.Quit();

        private void Awake()
        {
            saveManager.Initialize();
            saveManager.LoadOrCreateGameProgressData(gameData.LevelsCount);
            saveManager.LoadOrCreateSettingsData();
            contentManager.LoadContentFromResources(gameData.LevelsCount);
        }

        private void Start()
        {
            levelsManager.CreateLevelButtons(gameData.LevelsCount);
            statsManager.CreateLevelStatsCards();
            settingsManager.CreateSettingsViews();

            mainMenuSectionView.CreateButtons(buttonDatas);
            StartCoroutine(mainMenuSectionView.PlayStartAnimation_COR());
        }
    }
}