using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace GameLogic
{
    public class HandbookComponent : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private CanvasGroup _mainThemeButtonsContainer;
        [SerializeField] private CanvasGroup _subThemeButtonsContainer;
        [SerializeField] private Button _returnToMainThemesButton;
        [SerializeField] private Button _closeViewButton;

        public void SetMainThemeButtonsContainerScrollbarValue(float value) => SetContainerScrollbarValue(_mainThemeButtonsContainer.gameObject, value);

        public void SetSubThemeButtonsContainerScrollbarValue(float value) => SetContainerScrollbarValue(_subThemeButtonsContainer.gameObject, value);

        private void SetContainerScrollbarValue(GameObject buttonsContainer, float value)
        {
            var scrollbar = buttonsContainer.GetComponentInChildren<Scrollbar>();
            if (scrollbar != null)
            {
                scrollbar.value = value;
            }
        }



        public event Action<TrainingData[]> SubThemeButtonPressed;

        private const string HIDDEN_SUB_THEME_PLACEHOLDER = "???";
        private const float VISIBILITY_CHANGING_DURATION = 1.5f;

        private HandbookThemeButton _themeButtonPrefab;

        public HandbookComponent()
        {
            _closeViewButton.onClick.AddListener(OnCloseViewButtonPressed);
            _returnToMainThemesButton.onClick.AddListener(OnReturnToMainThemesButtonPressed);
        }

        public async UniTask ShowModalSectionAsync()
        {
            gameObject.SetActive(true);
            _returnToMainThemesButton.gameObject.SetActive(false);
            SetMainThemeButtonsContainerScrollbarValue(1f);

            await _canvasGroup.DOFade(1f, VISIBILITY_CHANGING_DURATION);
        }

        public async UniTask HideModalSectionAsync()
        {
            await _canvasGroup.DOFade(0f, VISIBILITY_CHANGING_DURATION);
            gameObject.SetActive(false);
        }

        public void FillStartTrainingThemes(TrainingTheme[] trainingThemes, TrainingSubTheme[] currentLevelTrainingSubThemes)
        {
            var mainThemeButtonDatas = new List<(HandbookThemeButton mainThemeButton, HandbookSubThemeContainerModel subThemesContainerModel)>();

            for (var i = 0; i < trainingThemes.Length; i++)
            {
                var mainThemeButton = Object.Instantiate(_themeButtonPrefab, _mainThemeButtonsContainer.transform);
                mainThemeButton.SetThemeText(trainingThemes[i].Title.GetLocalizedString());
                mainThemeButton.ButtonComponent.onClick.AddListener(() => OnMainThemeButtonPressed(mainThemeButton));

                var subThemeButtonDatas = new List<(HandbookThemeButton subThemeButton, TrainingSubTheme trainingSubTheme)>();
                var trainingSubThemeButtonsContainer = Object.Instantiate(_handbookThemeButtonContainerViewPrefab, _subThemeButtonsContainer.transform);
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
            _returnToMainThemesButton.gameObject.SetActive(false);

            await ChangeThemeButtonsContainerAsync(_subThemeButtonsContainer, _mainThemeButtonsContainer);
            _subThemeButtonsContainer.gameObject.SetActive(false);
        }

        private async UniTask ChangeThemeButtonsContainerAsync(CanvasGroup previousContainer, CanvasGroup newContainer)
        {
            await previousContainer.DOFade(0f, 0.75f).AsyncWaitForCompletion();
            await newContainer.DOFade(1f, 0.75f).AsyncWaitForCompletion();
        }

        private void OnMainThemeButtonPressed(HandbookThemeButton mainThemeButton)
        {
            _subThemeButtonsContainer.gameObject.SetActive(true);
            _returnToMainThemesButton.gameObject.SetActive(true);
            SetSubThemeButtonsContainerScrollbarValue(1f);
            ChangeThemeButtonsContainerAsync(_mainThemeButtonsContainer, _subThemeButtonsContainer).Forget();
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
