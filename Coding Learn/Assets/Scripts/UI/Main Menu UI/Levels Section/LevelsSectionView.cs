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
        private Image loadingBar;
        [SerializeField] 
        private TMP_Text loadingBarText;
        [Space, SerializeField]
        private LevelButton levelButtonPrefab;
        [SerializeField]
        private GameObject levelButtonsContainer;
        [SerializeField]
        private LevelThumbnail levelThumbnailPrefab;
        [SerializeField]
        private GameObject levelThumbnailContainer;
        [Space, SerializeField]
        private LevelsSectionAnimator animator;

        public IEnumerator ChangeVisibility_COR(bool isVisible)
        {
            yield return StartCoroutine(animator.ChangeContentVisibility_COR(isVisible));
        }

        public void CreateLevelButtons(int totalLevelsCount, int availableLevelsCount, Action<int> buttonPressedAction)
        {
            for (var i = 1; i <= totalLevelsCount; i++)
            {
                var currentLevelNumber = i;

                var chooseLevelButton = Instantiate(levelButtonPrefab, levelButtonsContainer.transform);
                chooseLevelButton.SetInfo(currentLevelNumber, "Здесь передаётся описание уровня");
                chooseLevelButton.SetButtonParams(currentLevelNumber == 1 || currentLevelNumber <= availableLevelsCount,  
                                                    () => buttonPressedAction(currentLevelNumber));
            }
        }

        public void SetLevelInfo(string levelTitle)
        {
            levelTitleLabel.text = levelTitle;
        }

        public void SetLoadingBarInfo(string loadingLabelText, float loadingOperationProgress)
        {
            loadingBar.fillAmount = loadingOperationProgress;
            loadingBarText.text = string.Format(@"{0}... {1}%", loadingLabelText, Mathf.Round(loadingOperationProgress * 100));
        }

        public void SetLevelThumbnail(Sprite newThumbnail)
        {
            var newLevelThumbnail = Instantiate(levelThumbnailPrefab, levelThumbnailContainer.transform);
            newLevelThumbnail.SetContent(newThumbnail);
            if (levelThumbnailContainer.transform.childCount > 1)
            {
                StartCoroutine(UpdateLevelThumbnail_COR());
            }           
            
        }

        public void MakeLevelButtonSelected(int buttonNumber) => levelButtonsContainer.transform.GetChild(buttonNumber - 1).GetComponent<LevelButton>().Select();

        private IEnumerator UpdateLevelThumbnail_COR()
        {
            var previousLevelThumbnail = levelThumbnailContainer.transform.GetChild(0).GetComponent<LevelThumbnail>();
            var newLevelThumbnail = levelThumbnailContainer.transform.GetChild(1).GetComponent<LevelThumbnail>();
            yield return animator.ChangeLevelThumbnail_COR(previousLevelThumbnail, newLevelThumbnail);
            Destroy(previousLevelThumbnail.gameObject);
        }
    }
}
