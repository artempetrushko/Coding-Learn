using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class PadHandbookAnimator : PadModalWindowAnimator
    {
        [SerializeField]
        private GameObject mainThemeButtonsContainer;
        [SerializeField]
        private GameObject subThemeButtonsContainer;

        public override IEnumerator ChangeVisibility_COR(bool isVisible)
        {
            windowVisibilityChangeTween ??= CreateWindowVisibilityChangeTween();
            if (isVisible)
            {
                windowVisibilityChangeTween.PlayForward();
                yield return windowVisibilityChangeTween.WaitForCompletion();
                yield return StartCoroutine(MoveButtonsContainer_COR(mainThemeButtonsContainer, -1));
            }
            else
            {
                windowVisibilityChangeTween.PlayBackwards();
                yield return windowVisibilityChangeTween.WaitForCompletion();
            }
        }

        public IEnumerator GoToSubThemeButtons_COR()
        {
            yield return StartCoroutine(ChangeThemeButtonsContainer_COR(mainThemeButtonsContainer, subThemeButtonsContainer, -1));
        }

        public IEnumerator ReturnToMainThemeButtons_COR()
        {
            yield return StartCoroutine(ChangeThemeButtonsContainer_COR(subThemeButtonsContainer, mainThemeButtonsContainer, 1));
        }

        private IEnumerator ChangeThemeButtonsContainer_COR(GameObject previousContainer, GameObject newContainer, int movementOffsetXSign)
        {
            yield return StartCoroutine(MoveButtonsContainer_COR(previousContainer, movementOffsetXSign));
            yield return StartCoroutine(MoveButtonsContainer_COR(newContainer, movementOffsetXSign));
        }

        private IEnumerator MoveButtonsContainer_COR(GameObject container, int movementOffsetXSign)
        {
            var movementTween = container.transform.DOLocalMoveX(container.transform.localPosition.x + container.GetComponent<RectTransform>().rect.width * movementOffsetXSign, 0.75f);
            yield return movementTween.WaitForCompletion();
        }
    }
}
