using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class ErrorsSectionAnimator : MonoBehaviour
    {
        public IEnumerator ChangeVisibility_COR(bool isVisible)
        {
            var movementOffsetYSign = isVisible ? 1 : -1;
            var movementTween = transform.DOLocalMoveY(transform.localPosition.y + transform.GetComponent<RectTransform>().sizeDelta.y * movementOffsetYSign, 1.5f);
            yield return movementTween.WaitForCompletion();
        }
    }
}
