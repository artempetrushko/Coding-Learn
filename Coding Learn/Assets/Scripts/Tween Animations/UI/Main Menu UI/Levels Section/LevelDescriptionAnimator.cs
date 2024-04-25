using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class LevelDescriptionAnimator : MonoBehaviour
    {
        [SerializeField]
        private Image[] backgroundParts;
        [SerializeField]
        private TMP_Text descriptionText;

        public async UniTask ChangeVisibilityAsync(bool isVisible)
        {
            var backgroundShowingTime = 0.3f;
            foreach (var part in backgroundParts)
            {
                part.DOFillAmount(isVisible ? 1f : 0f, backgroundShowingTime);
            }
            await UniTask.Delay(TimeSpan.FromSeconds(backgroundShowingTime));
            descriptionText.DOFade(1, 0.15f);
        }
    }
}
