using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

namespace Scripts
{
    public class LevelsSectionView : MonoBehaviour
    {
        [Header("UI-элементы")]
        [SerializeField] private TMP_Text levelTitleLabel;
        [SerializeField] private Button playButton;
        [SerializeField] private Image levelWallpapers;
        [SerializeField] private Image newLevelWallpapers;
        [SerializeField] private GameObject levelButtons;
        [SerializeField] private Image loadingBar;
        [SerializeField] private TMP_Text loadingBarText;

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

        private int currentLevelNumber = 0;
        private MenuLocalizationScript menuLocalization;

        public void CreateChooseLevelButtons(int totalLevelsCount, int availableLevelsCount, Action<int> buttonPressedAction)
        {
            for (var i = 1; i <= totalLevelsCount; i++)
            {
                var currentLevelNumber = i;

                var chooseLevelButton = Instantiate(chooseLevelButtonPrefab, levelButtonsContainer.transform);
                chooseLevelButton.SetInfo(currentLevelNumber, menuLocalization.GetLevelInfo(i).Description);
                chooseLevelButton.SetButtonParams(currentLevelNumber == 1 || currentLevelNumber <= availableLevelsCount, levelsPanelData.LevelButtonNormalColor, levelsPanelData.LevelButtonSelectedColor, 
                                                    () => buttonPressedAction(currentLevelNumber));
            }
        }

        public void SetLevelInfo(string levelTitle, string playButtonLabelText)
        {
            levelTitleLabel.text = levelTitle;
            playButton.GetComponentInChildren<TMP_Text>().text = playButtonLabelText;
            //StartCoroutine(SwitchLevelWallpapers_COR());
        }

        public void SetLoadingBarInfo(string loadingLabelText, float loadingOperationProgress)
        {
            loadingBar.fillAmount = loadingOperationProgress;
            loadingBarText.text = string.Format(@"{0}... {1}%", loadingLabelText, Mathf.Round(loadingOperationProgress * 100));
        }

        public void SetLevelThumbnail(Sprite thumbnail)
        {
            Destroy(levelThumbnailContainer.transform.GetChild(0).gameObject);

            var newLevelThumbnail = Instantiate(levelThumbnailPrefab, levelThumbnailContainer.transform);
            newLevelThumbnail.Enable(thumbnail);
        }

        public void CallChooseLevelButtonEvent(int buttonNumber) => levelButtonsContainer.transform.GetChild(buttonNumber - 1).GetComponent<ChooseLevelButton>().ClickForce();

        /*private IEnumerator SwitchLevelWallpapers_COR()
        {
            newLevelWallpapers.sprite = levelsPanelData.LoadingScreens[currentLevelNumber - 1];
            yield return StartCoroutine(PlayAnimation_COR(gameObject, "SwitchLevelWallpapers"));
            levelWallpapers.sprite = levelsPanelData.LoadingScreens[currentLevelNumber - 1];
            yield return StartCoroutine(PlayAnimation_COR(gameObject, "ReplaceImages"));
        }*/

        private IEnumerator PlayAnimation_COR(GameObject animatorHolder, string animationName)
        {
            var animator = animatorHolder.GetComponent<Animator>();
            var clip = animator.runtimeAnimatorController.animationClips.Where(x => x.name == animationName).First();
            animator.Play(clip.name);
            yield return new WaitForSeconds(clip.length);
        }

        private void Awake()
        {
            menuLocalization = transform.parent.GetComponent<MenuLocalizationScript>();
        }
    }
}
