using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class RewardingSectionAnimator : MonoBehaviour
    {
        public IEnumerator ChangeVisibility_COR(bool isVisible)
        {
            var scalingTween = transform.DOScale(isVisible ? 1f : 0f, 1.5f);
            yield return scalingTween.WaitForCompletion();
        }
    }
}
