using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class LevelDescriptionAnimator : MonoBehaviour
    {
        [SerializeField]
        private Image[] backgroundParts;

        public IEnumerator ChangeVisibility_COR(bool isVisible)
        {
            foreach (var part in backgroundParts)
            {
                part.DOFillAmount(isVisible ? 1f : 0f, 0.3f);
            }
            yield return new WaitForSeconds(0.3f);
        }
    }
}
