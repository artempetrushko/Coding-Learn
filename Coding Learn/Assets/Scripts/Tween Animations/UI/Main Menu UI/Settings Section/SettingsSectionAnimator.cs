using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class SettingsSectionAnimator : MonoBehaviour
    {
        public IEnumerator ChangeVisibility_COR(bool isVisible)
        {
            var visibilityChangeTween = transform.DOLocalMoveY(isVisible ? 0 : GetComponent<RectTransform>().rect.height, 0.75f);
            yield return visibilityChangeTween.WaitForCompletion();
        }
    }
}
