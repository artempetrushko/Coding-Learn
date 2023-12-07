using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts
{
    public class LevelsManager : MainMenuSectionManager
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

        public override IEnumerator ShowSectionView_COR()
        {
            levelsSectionView.gameObject.SetActive(true);
            SetLevelsSectionStartState();
            yield return StartCoroutine(levelsSectionView.ChangeVisibility_COR(true));
        }

        public override IEnumerator HideSectionView_COR()
        {
            yield return StartCoroutine(levelsSectionView.ChangeVisibility_COR(false));
            levelsSectionView.gameObject.SetActive(false);
        }

        public void CreateLevelButtons(int totalLevelsCount)
        {
            var availableLevelsDescriptions = MainMenuContentManager.GetAvailableLevelInfos(MainMenuSaveManager.GameProgressData.LastAvailableLevelNumber)
                .Select(levelInfo => levelInfo.Description)
                .ToArray();
            levelsSectionView.CreateLevelButtons(totalLevelsCount, availableLevelsDescriptions, ChangeLevelInfo);
        }

        public void SetLevelsSectionStartState()
        {
            var lastAvailableLevelNumber = MainMenuSaveManager.GameProgressData.LastAvailableLevelNumber;
            levelsSectionView.MakeLevelButtonSelected(lastAvailableLevelNumber);
        }

        public void LoadSelectedLevel() => StartCoroutine(LoadLevelAsync_COR());

        private void ChangeLevelInfo(int levelNumber) => SelectedLevelNumber = levelNumber;

        private void SetLevelInfo(int levelNumber)
        {
            var selectedLevelTitle = MainMenuContentManager.GetLevelInfo(levelNumber).LevelTitle;
            levelsSectionView.SetLevelInfo(selectedLevelTitle);
            levelsSectionView.SetLevelThumbnail(MainMenuContentManager.GetLoadingScreen(levelNumber));
        }

        private IEnumerator LoadLevelAsync_COR()
        {
            yield return StartCoroutine(levelsSectionView.ShowLoadingScreenContent_COR());

            var operation = SceneManager.LoadSceneAsync(SelectedLevelNumber);
            while (!operation.isDone)
            {
                levelsSectionView.SetLoadingBarInfo(operation.progress);
                yield return null;
            }
        }
    }
}
