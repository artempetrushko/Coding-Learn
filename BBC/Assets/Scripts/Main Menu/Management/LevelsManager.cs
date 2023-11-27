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
        private int selectedLevelNumber = 0;

        public int LevelsCount => levelsCount;

        public void SetLevelsSectionStartState()
        {
            if (!isLevelButtonsCreated)
            {
                levelsSectionView.CreateLevelButtons(levelsCount, MainMenuSaveManager.SaveData.LastAvailableLevelNumber, ChangeLevelInfo);
                isLevelButtonsCreated = true;
            }
            SetLevelInfo(selectedLevelNumber);
            levelsSectionView.MakeLevelButtonSelected(selectedLevelNumber);
        }

        public void LoadSelectedLevel() => StartCoroutine(LoadLevelAsync_COR());

        public void ChangeLevelInfo(int levelNumber)
        {
            if (levelNumber != selectedLevelNumber)
            {
                selectedLevelNumber = levelNumber;              
                SetLevelInfo(levelNumber);
            }
        }

        private void Start()
        {       
            selectedLevelNumber = MainMenuSaveManager.SaveData.LastAvailableLevelNumber;
        }

        private void SetLevelInfo(int levelNumber)
        {
            var selectedLevelTitle = MainMenuContentManager.GetLevelInfo(selectedLevelNumber).LevelTitle;
            levelsSectionView.SetLevelInfo(selectedLevelTitle);
            levelsSectionView.SetLevelThumbnail(MainMenuContentManager.GetLoadingScreen(levelNumber));
        }

        private IEnumerator LoadLevelAsync_COR()
        {
            var operation = SceneManager.LoadSceneAsync(selectedLevelNumber);
            while (!operation.isDone)
            {
                //levelsSectionView.SetLoadingBarInfo(menuLocalization.GetLoadBarText(), operation.progress);
                yield return null;
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
            throw new System.NotImplementedException();
        }
    }
}
