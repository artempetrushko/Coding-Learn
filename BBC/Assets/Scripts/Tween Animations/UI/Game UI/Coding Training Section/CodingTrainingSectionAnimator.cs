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
        private Image[] backgroundParts;

        public IEnumerator Show_COR()
        {
            yield return StartCoroutine(FillBackground_COR(1, 1));
            var contentShowingDuration = 0.7f;
            content.transform.DOLocalMoveY(390, contentShowingDuration);
            yield return new WaitForSeconds(contentShowingDuration);
        }

        public IEnumerator Hide_COR()
        {
            var contentShowingDuration = 1f;
            content.transform.DOLocalMoveY(1350, contentShowingDuration);
            yield return new WaitForSeconds(contentShowingDuration);
            yield return StartCoroutine(FillBackground_COR(0, 1));
        }

        private IEnumerator FillBackground_COR(float endFillAmount, float fillingDuration)
        {
            var everyPartFillingDuration = fillingDuration / backgroundParts.Length;
            foreach (var part in backgroundParts)
            {
                var fillingTween = part.DOFillAmount(endFillAmount, everyPartFillingDuration);
                fillingTween.Play();
                yield return fillingTween.WaitForCompletion();
            }       
        }
    }
}
