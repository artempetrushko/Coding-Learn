using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameLogic
{
    public class HandbookPresenter : IPadModalSection
    {
        public event Action<TrainingData[]> SubThemeButtonPressed;

        private const string HIDDEN_SUB_THEME_PLACEHOLDER = "???";
        private const float VISIBILITY_CHANGING_DURATION = 1.5f;

        private HandbookModel _handbookModel;
        private HandbookView _handbookView;
        private HandbookThemeButton _themeButtonPrefab;
        private HandbookThemeButtonContainerView _handbookThemeButtonContainerViewPrefab;

        public HandbookPresenter(HandbookView handbookView)
        {
            _handbookView = handbookView;

            _handbookView.CloseViewButton.onClick.AddListener(OnCloseViewButtonPressed);
            _handbookView.ReturnToMainThemesButton.onClick.AddListener(OnReturnToMainThemesButtonPressed);
        }

        public async UniTask ShowModalSectionAsync()
        {
            _handbookView.SetActive(true);
            _handbookView.ReturnToMainThemesButton.gameObject.SetActive(false);
            _handbookView.SetMainThemeButtonsContainerScrollbarValue(1f);

            await _handbookView.CanvasGroup.DOFade(1f, VISIBILITY_CHANGING_DURATION);
        }

        public async UniTask HideModalSectionAsync()
        {
            await _handbookView.CanvasGroup.DOFade(0f, VISIBILITY_CHANGING_DURATION);
            _handbookView.SetActive(false);
        }

        public void FillStartTrainingThemes(TrainingTheme[] trainingThemes, TrainingSubTheme[] currentLevelTrainingSubThemes)
        {
            var mainThemeButtonDatas = new List<(HandbookThemeButton mainThemeButton, HandbookSubThemeContainerModel subThemesContainerModel)>();

            for (var i = 0; i < trainingThemes.Length; i++)
            {
                var mainThemeButton = Object.Instantiate(_themeButtonPrefab, _handbookView.MainThemeButtonsContainer.transform);
                mainThemeButton.SetThemeText(trainingThemes[i].Title.GetLocalizedString());
                mainThemeButton.ButtonComponent.onClick.AddListener(() => OnMainThemeButtonPressed(mainThemeButton));

                var subThemeButtonDatas = new List<(HandbookThemeButton subThemeButton, TrainingSubTheme trainingSubTheme)>();
                var trainingSubThemeButtonsContainer = Object.Instantiate(_handbookThemeButtonContainerViewPrefab, _handbookView.SubThemeButtonsContainer.transform);
                for (var j = 0; j < trainingThemes[i].SubThemes.Length; j++)
                {
                    var subThemeButton = Object.Instantiate(_themeButtonPrefab, trainingSubThemeButtonsContainer.ButtonsContainer.transform);
                    subThemeButton.ButtonComponent.onClick.AddListener(() => OnSubThemeButtonPressed(subThemeButton));
                    if (currentLevelTrainingSubThemes.Contains(trainingThemes[i].SubThemes[j]))
                    {
                        subThemeButton.ButtonComponent.interactable = false;
                        subThemeButton.SetThemeText(HIDDEN_SUB_THEME_PLACEHOLDER);
                    }
                    else
                    {
                        subThemeButton.SetThemeText(trainingThemes[i].SubThemes[j].Title.GetLocalizedString());
                    }
                    subThemeButtonDatas.Add((subThemeButton, trainingThemes[i].SubThemes[j]));
                }
                var handbookSubThemeContainerModel = new HandbookSubThemeContainerModel(trainingSubThemeButtonsContainer, subThemeButtonDatas.ToArray());

                mainThemeButtonDatas.Add((mainThemeButton, handbookSubThemeContainerModel));
            }

            _handbookModel = new HandbookModel(mainThemeButtonDatas.ToArray());
        }

        public void MakeTrainingSubThemeAvailable(TrainingSubTheme trainingSubTheme)
        {
            var linkedSubThemeButton = _handbookModel.MainThemeButtonDatas
                .SelectMany(data => data.subThemesContainerModel.SubThemeButtonDatas)
                .First(data => data.trainingSubTheme == trainingSubTheme).subThemeButton;

            linkedSubThemeButton.SetThemeText(trainingSubTheme.Title.GetLocalizedString());
            linkedSubThemeButton.ButtonComponent.interactable = true;
        }

        public async UniTask ReturnToMainThemeButtonsAsync()
        {
            _handbookView.ReturnToMainThemesButton.gameObject.SetActive(false);

            await ChangeThemeButtonsContainerAsync(_handbookView.SubThemeButtonsContainer, _handbookView.MainThemeButtonsContainer);
            _handbookView.SubThemeButtonsContainer.gameObject.SetActive(false);

            _handbookModel.SelectedSubThemesContainer.ContainerView.gameObject.SetActive(false);
            _handbookModel.SelectedSubThemesContainer = null;
        }

        private async UniTask ChangeThemeButtonsContainerAsync(CanvasGroup previousContainer, CanvasGroup newContainer)
        {
            await previousContainer.DOFade(0f, 0.75f).AsyncWaitForCompletion();
            await newContainer.DOFade(1f, 0.75f).AsyncWaitForCompletion();
        }

        private void OnMainThemeButtonPressed(HandbookThemeButton mainThemeButton)
        {
            _handbookModel.SelectedSubThemesContainer = _handbookModel.MainThemeButtonDatas.First(mainThemeButtonData => mainThemeButtonData.mainThemeButton == mainThemeButton).subThemesContainerModel;
            _handbookModel.SelectedSubThemesContainer.ContainerView.gameObject.SetActive(true);

            _handbookView.SubThemeButtonsContainer.gameObject.SetActive(true);
            _handbookView.ReturnToMainThemesButton.gameObject.SetActive(true);
            _handbookView.SetSubThemeButtonsContainerScrollbarValue(1f);
            ChangeThemeButtonsContainerAsync(_handbookView.MainThemeButtonsContainer, _handbookView.SubThemeButtonsContainer).Forget();
        }

        private void OnSubThemeButtonPressed(HandbookThemeButton subThemeButton)
        {
            var selectedTrainingSubTheme = _handbookModel.SelectedSubThemesContainer.SubThemeButtonDatas.First(data => data.subThemeButton == subThemeButton).trainingSubTheme;
            var codingTrainingData = selectedTrainingSubTheme.TrainingDatas
                .Where(trainingData => trainingData.WillAddToHandbook)
                .ToArray();
            SubThemeButtonPressed?.Invoke(codingTrainingData);
        }

        private void OnReturnToMainThemesButtonPressed() => ReturnToMainThemeButtonsAsync().Forget();

        private void OnCloseViewButtonPressed() => HideModalSectionAsync().Forget();
    }
}
