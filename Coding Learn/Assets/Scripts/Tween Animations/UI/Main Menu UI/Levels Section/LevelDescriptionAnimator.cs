using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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

        public IEnumerator ChangeVisibility_COR(bool isVisible)
        {
            var backgroundShowingTime = 0.3f;
            foreach (var part in backgroundParts)
            {
                part.DOFillAmount(isVisible ? 1f : 0f, backgroundShowingTime);
            }
            yield return new WaitForSeconds(backgroundShowingTime);
            descriptionText.DOFade(1, 0.15f);
        }
    }
}
