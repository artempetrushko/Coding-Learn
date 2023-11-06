using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Scripts
{
    public class LevelsSectionView : MonoBehaviour
    {
        [SerializeField] 
        private TMP_Text levelTitleLabel;
        [SerializeField] 
        private Button playButton;
        [SerializeField] 
        private Image loadingBar;
        [SerializeField] 
        private TMP_Text loadingBarText;
        [Space]
        [SerializeField]
        private ChooseLevelButton chooseLevelButtonPrefab;
        [SerializeField]
        private GameObject levelButtonsContainer;
        [SerializeField]
        private LevelThumbnail levelThumbnailPrefab;
        [SerializeField]
        private GameObject levelThumbnailContainer;

        [SerializeField]
        private LevelsSectionData levelsPanelData;

        public void CreateChooseLevelButtons(int totalLevelsCount, int availableLevelsCount, Color normalColor, Color selectedColor, Action<int> buttonPressedAction)
        {
            for (var i = 1; i <= totalLevelsCount; i++)
            {
                var currentLevelNumber = i;

                var chooseLevelButton = Instantiate(chooseLevelButtonPrefab, levelButtonsContainer.transform);
                chooseLevelButton.SetInfo(currentLevelNumber, "Здесь передаётся описание уровня");
                chooseLevelButton.SetButtonParams(currentLevelNumber == 1 || currentLevelNumber <= availableLevelsCount, normalColor, selectedColor, 
                                                    () => buttonPressedAction(currentLevelNumber));
            }
        }

        public void SetLevelInfo(string levelTitle, string playButtonLabelText)
        {
            levelTitleLabel.text = levelTitle;
            playButton.GetComponentInChildren<TMP_Text>().text = playButtonLabelText;
        }

        public void SetLoadingBarInfo(string loadingLabelText, float loadingOperationProgress)
        {
            loadingBar.fillAmount = loadingOperationProgress;
            loadingBarText.text = string.Format(@"{0}... {1}%", loadingLabelText, Mathf.Round(loadingOperationProgress * 100));
        }

        public void SetLevelThumbnail(Sprite thumbnail)
        {
            var newLevelThumbnail = Instantiate(levelThumbnailPrefab, levelThumbnailContainer.transform);
            newLevelThumbnail.Enable(thumbnail);
            if (levelThumbnailContainer.transform.childCount > 1)
            {
                Destroy(levelThumbnailContainer.transform.GetChild(0).gameObject);
            }           
            
        }

        public void CallChooseLevelButtonEvent(int buttonNumber) => levelButtonsContainer.transform.GetChild(buttonNumber - 1).GetComponent<ChooseLevelButton>().ClickForce();
    }
}
