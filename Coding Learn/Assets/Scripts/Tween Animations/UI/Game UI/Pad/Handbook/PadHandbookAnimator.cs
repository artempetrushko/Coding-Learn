using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Scripts
{
    public class PadHandbookAnimator : PadModalWindowAnimator
    {
        [SerializeField]
        private GameObject mainThemeButtonsContainer;
        [SerializeField]
        private GameObject subThemeButtonsContainer;

        public override async UniTask ChangeVisibilityAsync(bool isVisible)
        {
            windowVisibilityChangeTween ??= CreateWindowVisibilityChangeTween();
            if (isVisible)
            {
                windowVisibilityChangeTween.PlayForward();
                await windowVisibilityChangeTween.AsyncWaitForCompletion();
                await MoveButtonsContainerAsync(mainThemeButtonsContainer, -1);
            }
            else
            {
                windowVisibilityChangeTween.PlayBackwards();
                await windowVisibilityChangeTween.AsyncWaitForCompletion();
            }
        }

        public async UniTask GoToSubThemeButtonsAsync() => await ChangeThemeButtonsContainerAsync(mainThemeButtonsContainer, subThemeButtonsContainer, -1);

        public async UniTask ReturnToMainThemeButtonsAsync() => await ChangeThemeButtonsContainerAsync(subThemeButtonsContainer, mainThemeButtonsContainer, 1);

        private async UniTask ChangeThemeButtonsContainerAsync(GameObject previousContainer, GameObject newContainer, int movementOffsetXSign)
        {
            await MoveButtonsContainerAsync(previousContainer, movementOffsetXSign);
            await MoveButtonsContainerAsync(newContainer, movementOffsetXSign);
        }

        private async UniTask MoveButtonsContainerAsync(GameObject container, int movementOffsetXSign)
        {
            await container.transform
                .DOLocalMoveX(container.transform.localPosition.x + container.GetComponent<RectTransform>().rect.width * movementOffsetXSign, 0.75f)
                .AsyncWaitForCompletion();
        }
    }
}
