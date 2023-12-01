using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class CodingTrainingSectionAnimator : MonoBehaviour
    {
        [SerializeField]
        private GameObject content;
        [SerializeField]
        private List<Image> backgroundParts;

        public IEnumerator Show_COR()
        {
            //yield return StartCoroutine(FillBackground_COR(1, 1));
            var contentShowingDuration = 0.7f;
            content.transform.DOLocalMoveY(390, contentShowingDuration);
            yield return new WaitForSeconds(contentShowingDuration);
        }

        public IEnumerator Hide_COR()
        {
            var contentShowingDuration = 1f;
            content.transform.DOLocalMoveY(1350, contentShowingDuration);
            yield return new WaitForSeconds(contentShowingDuration);
           // yield return StartCoroutine(FillBackground_COR(0, 1));
        }

        public IEnumerator ChangeVisibility_COR(bool isVisible)
        {
            var fillingDuration = 1f;
            var everyPartFillingDuration = fillingDuration / backgroundParts.Count;
            var endFillAmount = isVisible ? 1f : 0f;
            var contentEndPositionY = isVisible ? 0f : content.GetComponent<RectTransform>().rect.height + (Screen.height - content.GetComponent<RectTransform>().rect.yMax);

            var tweenSequence = DOTween.Sequence();
            tweenSequence.Pause();
            backgroundParts.ForEach(part => tweenSequence.Append(part.DOFillAmount(endFillAmount, everyPartFillingDuration)));
            tweenSequence.Append(content.transform.DOLocalMoveY(contentEndPositionY, 0.5f));

            if (isVisible)
            {
                tweenSequence.Play();
            }
            else
            {
                tweenSequence.PlayBackwards();
            }
            yield return tweenSequence.WaitForRewind();
        }

        /*private IEnumerator FillBackground_COR(float endFillAmount, float fillingDuration)
        {
            var everyPartFillingDuration = fillingDuration / backgroundParts.Length;
            foreach (var part in backgroundParts)
            {
                var fillingTween = part.DOFillAmount(endFillAmount, everyPartFillingDuration);
                fillingTween.Play();
                yield return fillingTween.WaitForCompletion();
            }       
        }*/
    }
}
