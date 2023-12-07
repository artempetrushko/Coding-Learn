using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class LoadingScreenAnimator : MonoBehaviour
    {
        [SerializeField]
        private Image background;

        public IEnumerator ShowBackground_COR()
        {
            var backgroundShowingTween = background.DOFade(1f, 1.5f);
            yield return backgroundShowingTween.WaitForCompletion();    
        }
    }
}
