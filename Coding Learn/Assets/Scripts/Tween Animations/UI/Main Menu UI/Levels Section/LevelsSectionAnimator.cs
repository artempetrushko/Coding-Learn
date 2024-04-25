using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class LevelsSectionAnimator : MonoBehaviour
    {
        [SerializeField]
        private GameObject header;
        [SerializeField]
        private GameObject levelButtonsContainer;
        [SerializeField]
        private CanvasGroup levelThumbnailContainer;
        [SerializeField]
        private Image blackScreen;
        [SerializeField]
        private GameObject loadingBarContainer;

        public async UniTask ChangeContentVisibilityAsync(bool isVisible)
        {
            var thumbnailContainerEndAlpha = isVisible ? 1f : 0f;
            var visibilityChangeDuration = 1f;

            ChangeMainContentVisibility(isVisible, visibilityChangeDuration);
            levelThumbnailContainer.DOFade(thumbnailContainerEndAlpha, visibilityChangeDuration);
            await UniTask.Delay(TimeSpan.FromSeconds(visibilityChangeDuration));
        }

        public async UniTask ChangeLevelThumbnailAsync(LevelThumbnail previousThumbnail, LevelThumbnail newThumbnail)
        {
            var thumbnailChangeDuration = 1f;
            previousThumbnail.Image.DOFillAmount(0f, thumbnailChangeDuration);
            newThumbnail.Image.DOFillAmount(1f, thumbnailChangeDuration);
            await UniTask.Delay(TimeSpan.FromSeconds(thumbnailChangeDuration));
        }

        public async UniTask ShowLoadingScreenAsync()
        {
            blackScreen.gameObject.SetActive(true);
            await ChangeBlackScreenVisibilityAsync(true);
            ChangeMainContentVisibility(false, 0f);
            loadingBarContainer.transform.DOLocalMoveY(loadingBarContainer.transform.localPosition.y + loadingBarContainer.GetComponent<RectTransform>().rect.height, 0f);
            await ChangeBlackScreenVisibilityAsync(false);
            blackScreen.gameObject.SetActive(false);
        }

        private async UniTask ChangeBlackScreenVisibilityAsync(bool isVisible)
        {
            var blackScreenEndOpacity = isVisible ? 1f : 0f;
            await blackScreen
                .DOFade(blackScreenEndOpacity, 1.5f)
                .AsyncWaitForCompletion();
        }

        private void ChangeMainContentVisibility(bool isVisible, float movementDuration)
        {
            var movementOffsetYSign = isVisible ? 1 : -1;
            MoveContentY(header, -movementOffsetYSign, movementDuration);
            MoveContentY(levelButtonsContainer, movementOffsetYSign, movementDuration);
        }

        private void MoveContentY(GameObject content, int movementSign, float movementDuration) 
            => content.transform.DOLocalMoveY(content.transform.localPosition.y + (content.GetComponent<RectTransform>().rect.height * movementSign), movementDuration);
    }
}
