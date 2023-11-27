using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class PadHandbookAnimator : MonoBehaviour
    {
        [SerializeField]
        private GameObject mainThemeButtonsContainer;
        [SerializeField]
        private GameObject subThemeButtonsContainer;

        public IEnumerator ShowHandbook_COR()
        {
            mainThemeButtonsContainer.transform.localPosition = new Vector3(mainThemeButtonsContainer.GetComponent<RectTransform>().sizeDelta.x, 0, 0);
            subThemeButtonsContainer.transform.localPosition = new Vector3(subThemeButtonsContainer.GetComponent<RectTransform>().sizeDelta.x, 0, 0);
            yield return StartCoroutine(ScaleHandbookView_COR(1));
            yield return StartCoroutine(MoveButtonsContainer_COR(mainThemeButtonsContainer, -1));
        }

        public IEnumerator HideHandbook_COR()
        {
            yield return StartCoroutine(ScaleHandbookView_COR(0));
        }

        public IEnumerator GoToSubThemeButtons_COR()
        {
            yield return StartCoroutine(ChangeThemeButtonsContainer_COR(mainThemeButtonsContainer, subThemeButtonsContainer));
        }

        public IEnumerator ReturnToMainThemeButtons_COR()
        {
            yield return StartCoroutine(ChangeThemeButtonsContainer_COR(subThemeButtonsContainer, mainThemeButtonsContainer));
        }

        private IEnumerator ScaleHandbookView_COR(float endScaleValue)
        {
            var scaleTween = transform.DOScale(endScaleValue, 1f);
            yield return scaleTween.WaitForCompletion();
        }

        private IEnumerator ChangeThemeButtonsContainer_COR(GameObject previousContainer, GameObject newContainer)
        {
            yield return StartCoroutine(MoveButtonsContainer_COR(previousContainer, -1));
            yield return StartCoroutine(MoveButtonsContainer_COR(newContainer, 1));
        }

        private IEnumerator MoveButtonsContainer_COR(GameObject container, int movementOffsetXSign)
        {
            var movementTween = container.transform.DOLocalMoveX(container.transform.localPosition.x + container.GetComponent<RectTransform>().sizeDelta.x * movementOffsetXSign, 1.5f);
            yield return movementTween.WaitForCompletion();
        }
    }
}
