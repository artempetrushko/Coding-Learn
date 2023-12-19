using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class MainMenuSectionAnimator : MonoBehaviour
    {
        [SerializeField]
        private GameObject content;
        [SerializeField]
        private List<Image> backgroundParts = new List<Image>();
        [SerializeField]
        private Image blackScreen;

        private Sequence mainMenuShowingTween;

        public IEnumerator HideBlackScreen_COR()
        {
            blackScreen.gameObject.SetActive(true);
            var blackScreenFadeOutTween = blackScreen.DOColor(new Color(0, 0, 0, 0), 2f);
            blackScreenFadeOutTween.Play();
            yield return blackScreenFadeOutTween.WaitForCompletion();
            blackScreen.gameObject.SetActive(false);
        }

        public IEnumerator ChangeMainMenuVisibility_COR(bool isVisible)
        {
            mainMenuShowingTween ??= CreateMainMenuShowingTween();
            if (isVisible)
            {
                content.transform.localPosition = new Vector3(0, content.GetComponent<RectTransform>().rect.height, 0);  
                mainMenuShowingTween.PlayForward();
            }
            else
            {
                mainMenuShowingTween.PlayBackwards();
            }
            yield return mainMenuShowingTween.WaitForRewind();
        }

        private Sequence CreateMainMenuShowingTween()
        {
            var backgroundFillingTotalDuration = 1.5f;
            var backgroundPartFillingDuration = backgroundFillingTotalDuration / backgroundParts.Count;
            content.transform.localPosition = new Vector3(0, content.GetComponent<RectTransform>().rect.height, 0);

            var tweenSequence = DOTween.Sequence();
            tweenSequence.Pause();
            backgroundParts.ForEach(part => tweenSequence.Append(DOTween.To(x => part.fillAmount = x, 0f, 1f, backgroundPartFillingDuration)));
            tweenSequence.Append(content.transform.DOLocalMoveY(0f, 0.5f));
            tweenSequence.SetAutoKill(false);
            return tweenSequence;
        }
    }
}
