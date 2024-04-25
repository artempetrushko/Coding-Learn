using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Scripts
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField]
        private MainMenuSectionView mainMenuSectionView;
        [SerializeField]
        private List<MainMenuButtonData> buttonDatas = new();
        [Space, SerializeField]
        private LevelsController levelsManager;
        [SerializeField]
        private StatsController statsManager;
        [SerializeField]
        private SettingsController settingsManager;
        [SerializeField]
        private MainMenuSaveManager saveManager;
        [SerializeField]
        private MainMenuContentManager contentManager;
        [Space, SerializeField]
        private GameData gameData;

        private IMainMenuSectionController currentMainMenuSection;

        [Inject]
        public void Construct(LevelsController levelsController, StatsController statsController, SettingsController settingsController)
        {
            levelsManager = levelsController;
            statsManager = statsController;
            settingsManager = settingsController;
        }

        public void ShowSelectedSection(IMainMenuSectionController section)
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
                await currentMainMenuSection.HideSectionViewAsync();
                await mainMenuSectionView.ShowContentAsync();
                currentMainMenuSection = null;
            });
        }

        public async UniTask ShowSelectedSectionAsync(IMainMenuSectionController section)
        {
            currentMainMenuSection = section;
            await mainMenuSectionView.HideContentAsync();
            await section.ShowSectionViewAsync();
        }

        public void Exit() => Application.Quit();

        public void RequestTextContentReload() => contentManager.LoadTextContent(MainMenuSaveManager.GameProgressData.LastAvailableLevelNumber);

        private void Awake()
        {
            saveManager.Initialize();
            saveManager.LoadOrCreateAllSavedData(gameData.LevelsCount);
            settingsManager.InitializeSettings();
            contentManager.LoadContentFromResources(MainMenuSaveManager.GameProgressData.LastAvailableLevelNumber);
        }

        private async void Start()
        {
            levelsManager.CreateLevelButtons(gameData.LevelsCount);
            statsManager.CreateLevelStatsCards();          

            mainMenuSectionView.CreateButtons(buttonDatas);
            await mainMenuSectionView.PlayStartAnimationAsync();
        }
    }
}