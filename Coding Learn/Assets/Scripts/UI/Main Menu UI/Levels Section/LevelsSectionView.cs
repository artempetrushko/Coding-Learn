using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class LevelsSectionView : MonoBehaviour
    {
        [SerializeField] private GameObject _header;
        [SerializeField] private TMP_Text _levelTitle;
        [SerializeField] private GameObject _levelButtonsContainer;       
        [SerializeField] private GameObject levelThumbnailContainer;
        [SerializeField] private Image _blackScreen;
        [SerializeField] private Image _loadingBar;
        [SerializeField] private TMP_Text _loadingBarText;
        [SerializeField] private GameObject _loadingBarContainer;

        public GameObject LevelButtonsContainer => _levelButtonsContainer;

        public void SetLevelTitleText(string text) => _levelTitle.text = text;

        public void SetLoadingBarFillAmount(float fillAmount) => _loadingBar.fillAmount = fillAmount;

        public void SetLoadingBarText(string text) => _loadingBarText.text = text;

        public void SetBlackScreenActive(bool isActive) => _blackScreen.gameObject.SetActive(isActive);

        public async UniTask SetBlackScreenVisibilityAsync(bool isVisible)
        {
            var blackScreenEndOpacity = isVisible ? 1f : 0f;
            await _blackScreen
                .DOFade(blackScreenEndOpacity, 1.5f)
                .AsyncWaitForCompletion();
        }

        public async UniTask SetHeaderVisibilityAsync(bool isVisible, float duration) => await SetContentVisibilityAsync(_header, isVisible, duration);

        public async UniTask SetLevelButtonsContainerVisibilityAsync(bool isVisible, float duration) => await SetContentVisibilityAsync(_levelButtonsContainer, isVisible, duration);

        private async UniTask SetContentVisibilityAsync(GameObject content, bool isVisible, float movementDuration)
        {
            var movementSign = isVisible ? 1 : -1;
            await content.transform
                .DOLocalMoveY(content.transform.localPosition.y + (content.GetComponent<RectTransform>().rect.height * movementSign), movementDuration)
                .AsyncWaitForCompletion();
        }

        public async UniTask SetLoadingBarContainerVisibilityAsync(bool isVisible, float duration)
        {
            var movementDirection = isVisible ? 1 : -1;
            await _loadingBarContainer.transform
                .DOLocalMoveY(_loadingBarContainer.transform.localPosition.y + _loadingBarContainer.GetComponent<RectTransform>().rect.height * movementDirection, duration)
                .AsyncWaitForCompletion();
        }
    }
}
