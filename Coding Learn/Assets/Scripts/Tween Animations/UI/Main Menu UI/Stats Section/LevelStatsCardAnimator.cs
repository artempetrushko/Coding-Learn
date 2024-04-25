using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
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

        public async UniTask ShowStarsCounter() => await ChangeStarsCounterVisibilityAsync(true);

        public async UniTask HideStarsCounter() => await ChangeStarsCounterVisibilityAsync(false);

        private async UniTask ChangeStarsCounterVisibilityAsync(bool isVisible)
        {
            starsCounterStartPositionY ??= starsCounter.transform.localPosition.y;
            var foregroundEndAlpha = isVisible ? 0.8f : 0f;
            var starsCounterEndPositionY = isVisible ? 0f : starsCounterStartPositionY;
            var visibilityChangeDuration = 0.2f;

            foreground.DOFade(foregroundEndAlpha, visibilityChangeDuration);
            starsCounter.transform.DOLocalMoveY(starsCounterEndPositionY.Value, visibilityChangeDuration);
            await UniTask.Delay(TimeSpan.FromSeconds(visibilityChangeDuration));
        }
    }
}
