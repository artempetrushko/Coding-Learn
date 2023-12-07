using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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

        public IEnumerator ChangeContentVisibility_COR(bool isVisible)
        {
            var thumbnailContainerEndAlpha = isVisible ? 1f : 0f;
            var visibilityChangeDuration = 1f;

            ChangeMainContentVisibility(isVisible, visibilityChangeDuration);
            levelThumbnailContainer.DOFade(thumbnailContainerEndAlpha, visibilityChangeDuration);
            yield return new WaitForSeconds(1f);
        }

        public IEnumerator ChangeLevelThumbnail_COR(LevelThumbnail previousThumbnail, LevelThumbnail newThumbnail)
        {
            var thumbnailChangeDuration = 1f;
            previousThumbnail.Image.DOFillAmount(0f, thumbnailChangeDuration);
            newThumbnail.Image.DOFillAmount(1f, thumbnailChangeDuration);
            yield return new WaitForSeconds(thumbnailChangeDuration);
        }

        public IEnumerator ShowLoadingScreen_COR()
        {
            blackScreen.gameObject.SetActive(true);
            yield return StartCoroutine(ChangeBlackScreenVisibility_COR(true));
            ChangeMainContentVisibility(false, 0f);
            loadingBarContainer.transform.DOLocalMoveY(loadingBarContainer.transform.localPosition.y + loadingBarContainer.GetComponent<RectTransform>().rect.height, 0f);
            yield return StartCoroutine(ChangeBlackScreenVisibility_COR(false));
            blackScreen.gameObject.SetActive(false);
        }

        private IEnumerator ChangeBlackScreenVisibility_COR(bool isVisible)
        {
            var blackScreenEndOpacity = isVisible ? 1f : 0f;
            var blackScreenShowingTween = blackScreen.DOFade(blackScreenEndOpacity, 1.5f);
            yield return blackScreenShowingTween.WaitForCompletion();
        }

        private void ChangeMainContentVisibility(bool isVisible, float movementDuration)
        {
            var movementOffsetYSign = isVisible ? 1f : -1f;
            header.transform.DOLocalMoveY(header.transform.localPosition.y - (header.GetComponent<RectTransform>().rect.height * movementOffsetYSign), movementDuration);
            levelButtonsContainer.transform.DOLocalMoveY(levelButtonsContainer.transform.localPosition.y + (header.GetComponent<RectTransform>().rect.height * movementOffsetYSign), movementDuration);
        }
    }
}
