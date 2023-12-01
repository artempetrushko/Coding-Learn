using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class PadViewsAnimator : MonoBehaviour
    {
        public IEnumerator ChangeViewVisibility_COR(GameObject view, bool isVisible)
        {
            var scalingTween = view.transform.DOScale(isVisible ? 1f : 0f, 1f);
            yield return scalingTween.WaitForCompletion();
        }
    }
}
