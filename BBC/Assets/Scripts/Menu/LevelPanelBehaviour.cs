using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

namespace Scripts
{
    public class LevelPanelBehaviour : MonoBehaviour
    {
        [SerializeField] private Text levelTitle;
        [SerializeField] private Button playButton;
        [SerializeField] private Image levelWallpapers;
        [SerializeField] private Image newLevelWallpapers;
        [SerializeField] private GameObject levelButtons;
        [SerializeField] private Image loadBar;
        [SerializeField] private Text loadBarText;
        [SerializeField] private List<Sprite> loadScreens = new List<Sprite>();
        [SerializeField] private Color normalButtonColor;
        [SerializeField] private Color normalHighlightedButtonColor;
        [SerializeField] private Color selectedButtonColor;
        [SerializeField] private Color selectedHighlightedButtonColor;
        private int currentLevelNumber = 0;
        private MenuLocalizationScript menuLocalization;

        public void LoadLevelAsync() => StartCoroutine(LoadLevelAsync_COR());

        public void ShowLevelInfo(int levelNumber)
        {
            if (levelNumber != currentLevelNumber && !GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("SwitchLevelWallpapers"))
            {
                ChangeCurrentButtonColors(normalButtonColor, normalHighlightedButtonColor);
                ChangeLevelInfo(levelNumber);
            }
        }

        public void ShowDefaultLevelInfo(int levelNumber)
        {
            FillLevelDescriptions();
            if (currentLevelNumber != 0)
                ChangeCurrentButtonColors(normalButtonColor, normalHighlightedButtonColor);
            ChangeLevelInfo(levelNumber);
        }

        public void ChangeLevelInfo(int levelNumber)
        {
            currentLevelNumber = levelNumber;
            ChangeCurrentButtonColors(selectedButtonColor, selectedHighlightedButtonColor);
            levelTitle.text = menuLocalization.GetLevelInfo(currentLevelNumber).LevelTitle;
            StartCoroutine(SwitchLevelWallpapers_COR());
        }

        public void FillLevelDescriptions()
        {
            for (var i = 1; i <= levelButtons.transform.childCount; i++)
                levelButtons.transform.GetChild(i - 1).GetChild(0).GetComponentInChildren<Text>().text = menuLocalization.GetLevelInfo(i).Description;
        }

        public IEnumerator LoadLevelAsync_COR()
        {
            var operation = SceneManager.LoadSceneAsync(currentLevelNumber == 1 ? SceneManager.sceneCountInBuildSettings - 1 : currentLevelNumber - 1);
            while (!operation.isDone)
            {
                loadBar.fillAmount = operation.progress;
                loadBarText.text = "Загрузка... " + (Mathf.Round(operation.progress * 100)) + "%";
                yield return null;
            }
        }

        private void ChangeCurrentButtonColors(Color newNormalColor, Color newHighlightedColor)
        {
            var currentLevelButtonColors = levelButtons.transform.GetChild(currentLevelNumber - 1).GetComponentInChildren<Button>().colors;
            currentLevelButtonColors.normalColor = newNormalColor;
            currentLevelButtonColors.highlightedColor = newHighlightedColor;
            currentLevelButtonColors.selectedColor = newNormalColor;
            levelButtons.transform.GetChild(currentLevelNumber - 1).GetComponentInChildren<Button>().colors = currentLevelButtonColors;
        }

        private IEnumerator SwitchLevelWallpapers_COR()
        {
            newLevelWallpapers.sprite = loadScreens[currentLevelNumber - 1];
            yield return StartCoroutine(PlayAnimation_COR(gameObject, "SwitchLevelWallpapers"));
            levelWallpapers.sprite = loadScreens[currentLevelNumber - 1];
            yield return StartCoroutine(PlayAnimation_COR(gameObject, "ReplaceImages"));
        }

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
