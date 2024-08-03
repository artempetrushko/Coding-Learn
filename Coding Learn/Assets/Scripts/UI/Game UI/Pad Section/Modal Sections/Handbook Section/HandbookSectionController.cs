using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Scripts
{
    public class HandbookSectionController
    {
        private HandbookSectionView _handbookSectionView;
        private HandbookThemeButton _themeButtonPrefab;
        private DiContainer _diContainer;

        public HandbookSectionController(DiContainer diContainer, HandbookSectionView handbookView)
        {
            _handbookSectionView = handbookView;
            _diContainer = diContainer;
        }

        public void SetVisibility(bool isVisible)
        {
            if (isVisible)
            {
                _handbookSectionView.SetPreviousHandbookPageButtonActive(false);
                _handbookSectionView.SetMainThemeButtonsContainerScrollbarValue(1f);

                ChangeVisibilityAsync(true);
            }
            else
            {
                ChangeVisibilityAsync(false);
            }
        }

        public async UniTask ShowSubThemeButtonsAsync()
        {
            _handbookSectionView.SetSubThemeButtonsContainerScrollbarValue(1f);
            await GoToSubThemeButtonsAsync();

            _handbookSectionView.SetPreviousHandbookPageButtonActive(true);
        }

        public async UniTask ReturnToMainThemeButtonsAsync()
        {
            _handbookSectionView.SetPreviousHandbookPageButtonActive(false);
            _handbookSectionView.SetMainThemeButtonsContainerScrollbarValue(1f);

            await ChangeThemeButtonsContainerAsync(subThemeButtonsContainer, mainThemeButtonsContainer, 1);
        }

        public void CreateThemeButtons<T>(T[] themes, Action<T> themeButtonPressedAction) where T : TrainingContent
        {
            var buttonsContainer = themes[0] switch
            {
                TrainingTheme => _handbookSectionView.MainThemeButtonsContainer,
                TrainingSubTheme => _handbookSectionView.SubThemeButtonsContainer
            };

            ClearButtonsContainer(buttonsContainer);

            for (var i = 1; i <= themes.Length; i++)
            {
                var themeNumber = i;
                var themeButton = _diContainer.InstantiatePrefab(_themeButtonPrefab, buttonsContainer.transform).GetComponent<HandbookThemeButton>();
                themeButton.SetThemeText(themes[themeNumber - 1].Title.GetLocalizedString());
                themeButton.ButtonComponent.onClick.AddListener(() => themeButtonPressedAction(themes[themeNumber - 1]));
            }

            SetContainerScrollbarDefaultValue(buttonsContainer);
        }

        private void ClearButtonsContainer(GameObject buttonsContainer)
        {
            for (var i = buttonsContainer.transform.childCount - 1; i >= 0; i--)
            {
                Object.Destroy(buttonsContainer.transform.GetChild(i).gameObject);
            }
            buttonsContainer.transform.DetachChildren();
        }

        






        public async UniTask ChangeVisibilityAsync(bool isVisible)
        {
            /*windowVisibilityChangeTween ??= CreateWindowVisibilityChangeTween();
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
            }*/
        }

        public async UniTask GoToSubThemeButtonsAsync() => await ChangeThemeButtonsContainerAsync(mainThemeButtonsContainer, subThemeButtonsContainer, -1);

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
