using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class ExitToMenuSectionAnimator : MonoBehaviour
    {
        [SerializeField]
        private Image background;
        [SerializeField]
        private GameObject contentContainer;
        [SerializeField]
        private Image blackScreen;

        public IEnumerator ChangeContentVisibility_COR(bool isVisible)
        {
            var backgroundEndOpacity = isVisible ? 0.9f : 0;
            var contentOpacity = isVisible ? 1 : 0;

            var tweenSequence = DOTween.Sequence();
            tweenSequence.Pause();
            tweenSequence.Append(background.DOFade(backgroundEndOpacity, 1f));
            tweenSequence.Append(contentContainer.GetComponent<CanvasGroup>().DOFade(contentOpacity, 0.75f));
            tweenSequence.SetUpdate(true);
            if (isVisible)
            {
                tweenSequence.Play();
            }
            else
            {
                tweenSequence.PlayBackwards();
            }
            yield return tweenSequence.WaitForCompletion();
        }

        public IEnumerator ShowBlackScreen_COR()
        {
            var blackScreenShowingTween = blackScreen.DOFade(1f, 2f).SetUpdate(true);
            yield return blackScreenShowingTween.WaitForCompletion();
        }
    }
}
