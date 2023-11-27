using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class LevelStatsCardAnimator : MonoBehaviour
    {
        [SerializeField]
        private Image foreground;
        [SerializeField]
        private GameObject starsCounter;

        private float? starsCounterStartPositionY;

        public void ShowStarsCounter() => StartCoroutine(ChangeStarsCounterVisibility_COR(true));

        public void HideStarsCounter() => StartCoroutine(ChangeStarsCounterVisibility_COR(false));

        private IEnumerator ChangeStarsCounterVisibility_COR(bool isVisible)
        {
            if (starsCounterStartPositionY == null)
            {
                starsCounterStartPositionY = starsCounter.transform.localPosition.y;
            }
            var foregroundEndAlpha = isVisible ? 0.8f : 0f;
            var starsCounterEndPositionY = isVisible ? 0f : starsCounterStartPositionY;
            var visibilityChangeDuration = 0.2f;

            foreground.DOFade(foregroundEndAlpha, visibilityChangeDuration);
            starsCounter.transform.DOLocalMoveY(starsCounterEndPositionY.Value, visibilityChangeDuration);
            yield return new WaitForSeconds(visibilityChangeDuration);
        }
    }
}
