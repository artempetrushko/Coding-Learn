using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts
{
    public class LevelsManager : MonoBehaviour
    {
        [SerializeField]
        private LevelsSectionView levelsSectionView;
        [SerializeField]
        private LevelsSectionData levelsSectionData;

        private MenuLocalizationScript menuLocalization;
        private bool isLevelButtonsCreated = false;
        private int selectedLevelNumber = 0;

        public void SetLevelsSectionStartState()
        {
            if (!isLevelButtonsCreated)
            {
                levelsSectionView.CreateChooseLevelButtons(10, SaveManager.SaveData.LastAvailableLevelNumber, levelsSectionData.LevelButtonNormalColor, levelsSectionData.LevelButtonSelectedColor, ShowLevelInfo);
                isLevelButtonsCreated = true;
            }
            levelsSectionView.CallChooseLevelButtonEvent(selectedLevelNumber);
        }

        public void LoadSelectedLevel() => StartCoroutine(LoadLevelAsync_COR());

        public void ShowLevelInfo(int levelNumber)
        {
            if (levelNumber != selectedLevelNumber)
            {
                selectedLevelNumber = levelNumber;
                var savedLevelNumberToResume = SaveManager.SaveData.LevelNumberToResume;                
                var selectedLevelTitle = menuLocalization.GetLevelInfo(selectedLevelNumber).LevelTitle;
                var newPlayButtonLabelText = savedLevelNumberToResume > 0 && selectedLevelNumber == savedLevelNumberToResume
                                                ? menuLocalization.GetPlayButtonText_SavedLevel()
                                                : menuLocalization.GetPlayButtonText();
                levelsSectionView.SetLevelInfo(selectedLevelTitle, newPlayButtonLabelText);
                levelsSectionView.SetLevelThumbnail(levelsSectionData.LoadingScreens[levelNumber - 1]);
            }
        }

        private void Start()
        {
            menuLocalization = GetComponent<MenuLocalizationScript>();            
            selectedLevelNumber = SaveManager.SaveData.LevelNumberToResume;
        }

        private IEnumerator LoadLevelAsync_COR()
        {
            var operation = SceneManager.LoadSceneAsync(selectedLevelNumber);
            while (!operation.isDone)
            {
                levelsSectionView.SetLoadingBarInfo(menuLocalization.GetLoadBarText(), operation.progress);
                yield return null;
            }
        }
    }
}
