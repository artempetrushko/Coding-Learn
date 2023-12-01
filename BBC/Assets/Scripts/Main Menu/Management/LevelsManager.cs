using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts
{
    public class LevelsManager : MainMenuSectionManager
    {
        [SerializeField]
        private int levelsCount;
        [Space, SerializeField]
        private LevelsSectionView levelsSectionView;

        private bool isLevelButtonsCreated = false;
        private int selectedLevelNumber;

        public int LevelsCount => levelsCount;
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

        public void SetLevelsSectionStartState()
        {
            var lastAvailableLevelNumber = MainMenuSaveManager.SaveData.LastAvailableLevelNumber;
            if (!isLevelButtonsCreated)
            {
                levelsSectionView.CreateLevelButtons(levelsCount, lastAvailableLevelNumber, ChangeLevelInfo);
                isLevelButtonsCreated = true;
            }
            //SetLevelInfo(selectedLevelNumber);
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
            var operation = SceneManager.LoadSceneAsync(SelectedLevelNumber);
            while (!operation.isDone)
            {
                //levelsSectionView.SetLoadingBarInfo(menuLocalization.GetLoadBarText(), operation.progress);
                yield return null;
            }
        }
    }
}
