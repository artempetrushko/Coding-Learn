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

        public IEnumerator ChangeContentVisibility_COR(bool isVisible)
        {
            var backgroundEndColor = new Color(0, 0, 0, isVisible ? 0.9f : 0);
            var contentOpacity = isVisible ? 1 : 0;

            var tweenSequence = DOTween.Sequence();
            tweenSequence.Append(background.DOColor(backgroundEndColor, 1f));
            tweenSequence.Append(contentContainer.GetComponent<CanvasGroup>().DOFade(contentOpacity, 0.75f));
            tweenSequence.Play();
            yield return tweenSequence.WaitForCompletion();
        }
    }
}
