using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Scripts
{
    public class MainMenuBootstrap : MonoBehaviour
    {
        [SerializeField] private GameData gameData;
        [SerializeField] private List<MainMenuButtonData> buttonDatas = new();

        private MainMenuSectionController _mainMenuSectionController;
        private LevelsManager levelsManager;
        private StatsManager statsManager;
        private SettingsManager settingsManager;
        private SaveManager saveManager;

        private IMainMenuSectionManager currentMainMenuSection;

        [Inject]
        public void Construct(LevelsManager levelsController, StatsManager statsController, SettingsManager settingsController)
        {
            levelsManager = levelsController;
            statsManager = statsController;
            settingsManager = settingsController;
        }

        private void Awake()
        {
            saveManager.LoadOrCreateAllSavedData(gameData.LevelDatas.Length);
            settingsManager.InitializeSettings();
        }

        private async void Start()
        {
            levelsManager.CreateLevelButtons(gameData.LevelDatas.Length);
            statsManager.CreateLevelStatsCards();

            _mainMenuSectionController.CreateButtons(buttonDatas);
            await _mainMenuSectionController.PlayStartAnimationAsync();
        }

        public void ShowSelectedSection(IMainMenuSectionManager section)
        {
            /*UniTask.Void(async (section) =>
            {
                currentMainMenuSection = section;
                await mainMenuSectionView.HideContentAsync();
                await section.ShowSectionViewAsync();
            });*/
        }

        public void HideCurrentSection()
        {
            UniTask.Void(async () =>
            {
                await currentMainMenuSection.HideSectionAsync();
                await _mainMenuSectionController.ShowContentAsync();
                currentMainMenuSection = null;
            });
        }

        public async UniTask ShowSelectedSectionAsync(IMainMenuSectionManager section)
        {
            currentMainMenuSection = section;
            await _mainMenuSectionController.HideContentAsync();
            await section.ShowSectionAsync();
        }

        public void Exit() => Application.Quit();

        //public void RequestTextContentReload() => contentManager.LoadTextContent(saveManager.GameProgressData.LastAvailableLevelNumber);

        
    }
}