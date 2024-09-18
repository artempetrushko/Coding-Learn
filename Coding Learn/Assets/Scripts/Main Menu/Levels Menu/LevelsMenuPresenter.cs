using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameLogic;
using UI.MainMenu;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace MainMenu
{
    public class LevelsMenuPresenter : IMainMenuSectionPresenter, IDisposable
    {
        public event Action SectionDisabled;
        public event Action<LevelConfig> LevelSelected;

        private const float SECTION_VISIBILITY_CHANGING_DURATION = 1f;

        private LevelsMenuConfig _levelsSectionConfig;
        private LevelsMenuModel _levelsMenuModel;
        private LevelsMenuView _levelsMenuView;
        private LevelButton _levelButtonPrefab;

        public LevelsMenuPresenter(LevelsMenuView levelsMenuView, LevelsMenuConfig levelsSectionConfig, LevelButton levelButtonPrefab)
        {
            _levelsMenuView = levelsMenuView;
            _levelsSectionConfig = levelsSectionConfig;
            _levelButtonPrefab = levelButtonPrefab;

            _levelsMenuView.PlayButton.onClick.AddListener(OnPlayButtonPressed);
            _levelsMenuView.CloseViewButton.onClick.AddListener(OnCloseViewButtonPressed);
        }

        public void Dispose()
        {
            foreach (var levelButtonConfig in _levelsMenuModel.LevelButtonConfigs)
            {
                levelButtonConfig.levelButton.PointerEnter -= OnLevelButtonPointerEnter;
                levelButtonConfig.levelButton.PointerExit -= OnLevelButtonPointerExit;
                levelButtonConfig.levelButton.ButtonSelected -= OnLevelButtonSelected;
                levelButtonConfig.levelButton.ButtonDeselected -= OnLevelButtonDeselected;
            }
        }

        public async UniTask ShowSectionAsync()
        {
            _levelsMenuView.SetActive(true);
            await ShowLevelInfoAsync(_levelsMenuModel.SelectedLevelConfig);
            await SetSectionVisibilityAsync(true);
        }

        public async UniTask HideSectionAsync()
        {
            await SetSectionVisibilityAsync(false);
            _levelsMenuView.SetActive(false);

            SectionDisabled?.Invoke();
        }

        public void Initialize(LevelConfig[] levelConfigs, int lastAvailableLevelNumber)
        {
            _levelsMenuModel = new LevelsMenuModel(levelConfigs)
            {
                SelectedLevelConfig = levelConfigs[lastAvailableLevelNumber - 1]
            };

            CreateLevelButtons(lastAvailableLevelNumber);
        }

        private async UniTask SetSectionVisibilityAsync(bool isVisible)
        {
            SetContentVisibilityAsync(_levelsMenuView.Header, isVisible ? -1 : 1, SECTION_VISIBILITY_CHANGING_DURATION).Forget();
            SetContentVisibilityAsync(_levelsMenuView.LevelButtonsContainer, isVisible ? 1 : -1, SECTION_VISIBILITY_CHANGING_DURATION).Forget();
            _levelsMenuView.LevelThumbnailView.Thumbnail.DOFade(isVisible ? 1f : 0f, SECTION_VISIBILITY_CHANGING_DURATION).ToUniTask().Forget();

            await UniTask.WaitForSeconds(SECTION_VISIBILITY_CHANGING_DURATION);
        }

        private async UniTask SetContentVisibilityAsync(GameObject content, int movementSign, float movementDuration)
        {
            await content.transform
                .DOLocalMoveY(content.transform.localPosition.y + (content.GetComponent<RectTransform>().rect.height * movementSign), movementDuration)
                .AsyncWaitForCompletion();
        }

        private async UniTask ShowLevelInfoAsync(LevelConfig levelConfig)
        {
            _levelsMenuView.SetLevelTitleText(levelConfig.Title.GetLocalizedString());
            await ChangeLevelThumbnailAsync(levelConfig.ThumbnailReference);
        }

        private void CreateLevelButtons(int lastAvailableLevelNumber)
        {
            var levelButtonConfigs = new (LevelButton levelbutton, LevelConfig linkedLevelConfig)[lastAvailableLevelNumber];
            for (var i = 1; i <= _levelsMenuModel.LevelConfigs.Length; i++)
            {
                var levelButton = Object.Instantiate(_levelButtonPrefab, _levelsMenuView.LevelButtonsContainer.transform);
                levelButton.SetLevelNumberLabelText(i.ToString());

                var isButtonInteractable = i <= lastAvailableLevelNumber;
                levelButton.SetInteractable(isButtonInteractable);
                if (isButtonInteractable)
                {
                    levelButton.ButtonComponent.onClick.AddListener(() => OnLevelButtonPressed(levelButton));
                    levelButton.PointerEnter += OnLevelButtonPointerEnter;
                    levelButton.PointerExit += OnLevelButtonPointerExit;
                    levelButton.ButtonSelected += OnLevelButtonSelected;
                    levelButton.ButtonDeselected += OnLevelButtonDeselected;

                    levelButtonConfigs[i] = (levelButton, _levelsMenuModel.LevelConfigs[i]);
                }
            }

            _levelsMenuModel.LevelButtonConfigs = levelButtonConfigs;
        }

        private async UniTask ChangeLevelThumbnailAsync(AssetReference thumbnailReference)
        {
            var newThumbnail = await Addressables.LoadAssetAsync<Sprite>(thumbnailReference);

            _levelsMenuView.TransitionThumbnailView.SetActive(true);
            _levelsMenuView.TransitionThumbnailView.CanvasGroup.alpha = 1f;
            _levelsMenuView.TransitionThumbnailView.Thumbnail.sprite = _levelsMenuView.LevelThumbnailView.Thumbnail.sprite;
            _levelsMenuView.LevelThumbnailView.Thumbnail.sprite = newThumbnail;

            await _levelsMenuView.TransitionThumbnailView.CanvasGroup.DOFade(0f, _levelsSectionConfig.ThumbnailChangingDuration).AsyncWaitForCompletion();
            _levelsMenuView.TransitionThumbnailView.SetActive(false);
        }

        private void SetLevelButtonScale(LevelButton levelButton, float scale)
        {
            levelButton.transform.DOScale(scale, _levelsSectionConfig.ButtonScaleDuration);
        }

        private async UniTask ShowLevelDescriptionViewAsync(string levelDescriptionText)
        {
            _levelsMenuView.LevelDescriptionView.SetActive(true);

            var backgroundParts = _levelsMenuView.LevelDescriptionView.BackgroundParts;
            for (var i = 0; i < backgroundParts.Length; i++)
            {
                await backgroundParts[i].DOFillAmount(1f, _levelsSectionConfig.LevelDescriptionVisibilityChangingDuration).AsyncWaitForCompletion();
            }

            _levelsMenuView.LevelDescriptionView.SetDescriptionText(levelDescriptionText);
        }

        private async UniTask HideLevelDescriptionViewAsync()
        {
            _levelsMenuView.LevelDescriptionView.SetDescriptionText("");

            var backgroundParts = _levelsMenuView.LevelDescriptionView.BackgroundParts;
            for (var i = backgroundParts.Length - 1; i >= 0; i--)
            {
                await backgroundParts[i].DOFillAmount(0f, _levelsSectionConfig.LevelDescriptionVisibilityChangingDuration).AsyncWaitForCompletion();
            }

            _levelsMenuView.LevelDescriptionView.SetActive(false);
        }

        private void OnPlayButtonPressed() => LevelSelected?.Invoke(_levelsMenuModel.SelectedLevelConfig);

        private void OnCloseViewButtonPressed() => HideSectionAsync().Forget();

        private void OnLevelButtonPressed(LevelButton levelButton)
        {
            _levelsMenuModel.SelectedLevelConfig = _levelsMenuModel.LevelButtonConfigs.First(config => config.levelButton == levelButton).linkedLevelConfig;
            ShowLevelInfoAsync(_levelsMenuModel.SelectedLevelConfig).Forget();
        }

        private void OnLevelButtonPointerEnter(LevelButton levelButton)
        {
            SetLevelButtonScale(levelButton, _levelsSectionConfig.PointerEnterButtonScale);

            var levelDescription = _levelsMenuModel.LevelButtonConfigs.First(config => config.levelButton == levelButton).linkedLevelConfig.Description.GetLocalizedString();
            ShowLevelDescriptionViewAsync(levelDescription).Forget();
        }

        private void OnLevelButtonPointerExit(LevelButton levelButton)
        {
            SetLevelButtonScale(levelButton, 1f);
            HideLevelDescriptionViewAsync().Forget();
        }

        private void OnLevelButtonSelected(LevelButton levelButton) => levelButton.SetInnerAreaColor(_levelsSectionConfig.ButtonSelectedColor);

        private void OnLevelButtonDeselected(LevelButton levelButton) => levelButton.SetInnerAreaColor(_levelsSectionConfig.ButtonNormalColor);
    }
}
