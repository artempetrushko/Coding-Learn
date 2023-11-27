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

        public IEnumerator ChangeContentVisibility_COR(bool isVisible)
        {
            var movementOffsetYSign = isVisible ? 1f : 0f;
            var thumbnailContainerEndAlpha = isVisible ? 1f : 0f;
            var visibilityChangeDuration = 1f;

            header.transform.DOLocalMoveY(header.transform.localPosition.y - (header.GetComponent<RectTransform>().rect.height * movementOffsetYSign), visibilityChangeDuration);
            levelButtonsContainer.transform.DOLocalMoveY(levelButtonsContainer.transform.localPosition.y + (header.GetComponent<RectTransform>().rect.height * movementOffsetYSign), visibilityChangeDuration);
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
    }
}
