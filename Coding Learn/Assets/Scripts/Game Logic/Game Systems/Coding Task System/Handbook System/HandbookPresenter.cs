using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UI.Game;
using UnityEngine;

namespace GameLogic
{
    public class HandbookPresenter : IPadModalSection
    {
        public event Action<TrainingData[]> SubThemeButtonPressed;

        private const float VISIBILITY_CHANGING_DURATION = 1.5f;

        private HandbookView _handbookSectionView;
        private HandbookThemeButton _themeButtonPrefab;

        private TrainingTheme currentTrainingTheme;
        private TrainingSubTheme currentTrainingSubTheme;
        private bool areMainThemeButtonsCreated = false;

        public HandbookPresenter(HandbookView handbookView)
        {
            _handbookSectionView = handbookView;
        }

        public async UniTask ShowModalSectionAsync()
        {
            _handbookSectionView.SetPreviousHandbookPageButtonActive(false);
            _handbookSectionView.SetMainThemeButtonsContainerScrollbarValue(1f);

            await _handbookSectionView.CanvasGroup.DOFade(1f, VISIBILITY_CHANGING_DURATION);
        }

        public async UniTask HideModalSectionAsync()
        {
            await _handbookSectionView.CanvasGroup.DOFade(0f, VISIBILITY_CHANGING_DURATION);
        }

        public void SetNewTrainingContent(TrainingTheme currentTrainingTheme, TrainingSubTheme currentTrainingSubTheme)
        {
            this.currentTrainingTheme = currentTrainingTheme;
            this.currentTrainingSubTheme = currentTrainingSubTheme;
            if (!areMainThemeButtonsCreated)
            {
                //CreateMainThemeButtons();
                areMainThemeButtonsCreated = true;
            }
        }



        private void CreateMainThemeButtons(TrainingTheme[] trainingThemes) { }//=> _handbookSectionController.CreateThemeButtons(trainingThemes, GoToSubThemeButtons);

        public void ReturnToMainThemeButtons() => ReturnToMainThemeButtonsAsync().Forget();

        private void GoToSubThemeButtons(TrainingTheme trainingMainTheme)
        {
            int? subThemesLimit = trainingMainTheme == currentTrainingTheme
                ? trainingMainTheme.SubThemes.ToList().IndexOf(currentTrainingSubTheme) + 1
                : null;
            var subThemes = FilterHandbookTrainingSubThemes(trainingMainTheme, subThemesLimit);
            //_handbookSectionController.CreateThemeButtons(subThemes, ShowSubThemeContent);
            ShowSubThemeButtonsAsync().Forget();
        }

        private void ShowSubThemeContent(TrainingSubTheme trainingSubTheme)
        {
            var codingTrainingDatas = trainingSubTheme.TrainingDatas
                .Where(trainingData => trainingData.WillAddToHandbook)
                .ToArray();
            SubThemeButtonPressed?.Invoke(codingTrainingDatas);
        }

        private TrainingSubTheme[] FilterHandbookTrainingSubThemes(TrainingTheme mainTheme, int? lastAvailableSubThemeNumber = null)
        {
            var subThemes = mainTheme.SubThemes;
            if (lastAvailableSubThemeNumber != null)
            {
                subThemes = subThemes.Take(lastAvailableSubThemeNumber.Value).ToArray();
            }
            return subThemes
                .Where(subTheme => subTheme.TrainingDatas.Any(data => data.WillAddToHandbook))
                .ToArray();
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

            //await ChangeThemeButtonsContainerAsync(subThemeButtonsContainer, mainThemeButtonsContainer, 1);
        }

        /*public void CreateThemeButtons<T>(T[] themes, Action<T> themeButtonPressedAction) where T : TrainingContent
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
                var themeButton = Object.Instantiate(_themeButtonPrefab, buttonsContainer.transform);
                themeButton.SetThemeText(themes[themeNumber - 1].Title.GetLocalizedString());
                themeButton.ButtonComponent.onClick.AddListener(() => themeButtonPressedAction(themes[themeNumber - 1]));
            }

            //SetContainerScrollbarDefaultValue(buttonsContainer);
        }*/


        public async UniTask GoToSubThemeButtonsAsync() { }//=> await ChangeThemeButtonsContainerAsync(mainThemeButtonsContainer, subThemeButtonsContainer, -1);

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
