using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts
{
    public class LevelsController : IMainMenuSectionController
    {
        [SerializeField]
        private LevelsSectionView levelsSectionView;

        private int selectedLevelNumber;

        private int SelectedLevelNumber
        {
            get => selectedLevelNumber;
            set
            {
                if (selectedLevelNumber != value)
                {
                    selectedLevelNumber = value;
                    SetLevelInfo(selectedLevelNumber);
                }
            }
        }

        public LevelsController(LevelsSectionView levelsSectionView)
        {
            this.levelsSectionView = levelsSectionView;
        }

        public async UniTask ShowSectionViewAsync()
        {
            levelsSectionView.gameObject.SetActive(true);
            SetLevelsSectionStartState();
            await levelsSectionView.ChangeVisibility_COR(true);
        }

        public async UniTask HideSectionViewAsync()
        {
            await levelsSectionView.ChangeVisibility_COR(false);
            levelsSectionView.gameObject.SetActive(false);
        }

        public void CreateLevelButtons(int totalLevelsCount) => levelsSectionView.CreateLevelButtons(totalLevelsCount);

        public void SetLevelsSectionStartState()
        {
            var lastAvailableLevelNumber = MainMenuSaveManager.GameProgressData.LastAvailableLevelNumber;
            var availableLevelsDescriptions = MainMenuContentManager.GetAvailableLevelInfos(lastAvailableLevelNumber)
                .Select(levelInfo => levelInfo.Description.GetLocalizedString())
                .ToArray();
            levelsSectionView.SetActiveLevelButtonsParams(availableLevelsDescriptions, ChangeLevelInfo);
            levelsSectionView.MakeLevelButtonSelected(lastAvailableLevelNumber);
            if (SelectedLevelNumber != lastAvailableLevelNumber)
            {
                SelectedLevelNumber = lastAvailableLevelNumber;
            }
            else
            {
                SetLevelInfo(lastAvailableLevelNumber);
            }
        }

        public void LoadSelectedLevel()
        {
            UniTask.Void(async () =>
            {
                await levelsSectionView.ShowLoadingScreenContentAsync();

                var operation = SceneManager.LoadSceneAsync(SelectedLevelNumber);
                while (!operation.isDone)
                {
                    levelsSectionView.SetLoadingBarInfo(operation.progress);
                    await UniTask.Yield();
                }
            });
        }

        private void ChangeLevelInfo(int levelNumber) => SelectedLevelNumber = levelNumber;

        private void SetLevelInfo(int levelNumber)
        {
            var selectedLevelTitle = MainMenuContentManager.GetLevelInfo(levelNumber).Title.GetLocalizedString();
            levelsSectionView.SetLevelInfo(selectedLevelTitle);
            levelsSectionView.SetLevelThumbnail(MainMenuContentManager.GetLoadingScreen(levelNumber));
        }
    }
}
