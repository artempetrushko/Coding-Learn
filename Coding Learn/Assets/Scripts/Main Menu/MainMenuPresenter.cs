using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameLogic;
using SaveSystem;
using UI.MainMenu;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class MainMenuPresenter : IDisposable
    {
        private GameConfig _gameConfig;
        private MainMenuView _mainMenuView;
        private LevelsMenuPresenter _levelsMenuPresenter;
        private StatisticsMenuPresenter _statisticsMenuPresenter;
        private SettingsMenuPresenter _settingsMenuPresenter;

        private GameProgress _gameProgress;
        private Sequence _mainMenuShowingTween;

        public MainMenuPresenter(GameConfig gameConfig, MainMenuView mainMenuView, LevelsMenuPresenter levelsMenuPresenter, StatisticsMenuPresenter statisticsMenuPresenter, SettingsMenuPresenter settingsMenuPresenter)
        {
            _gameConfig = gameConfig;
            _mainMenuView = mainMenuView;
            _levelsMenuPresenter = levelsMenuPresenter;
            _statisticsMenuPresenter = statisticsMenuPresenter;
            _settingsMenuPresenter = settingsMenuPresenter;

            BindMainMenuSectionWithButton(_levelsMenuPresenter, _mainMenuView.LevelsButton);
            BindMainMenuSectionWithButton(_statisticsMenuPresenter, _mainMenuView.StatisticsButton);
            BindMainMenuSectionWithButton(_settingsMenuPresenter, _mainMenuView.SettingsButton);

            _mainMenuView.ExitButton.onClick.AddListener(OnExitButtonClicked);
        }

        public void Dispose()
        {
            _levelsMenuPresenter.SectionDisabled -= OnMainMenuSectionDisabled;
            _statisticsMenuPresenter.SectionDisabled -= OnMainMenuSectionDisabled;
            _settingsMenuPresenter.SectionDisabled -= OnMainMenuSectionDisabled;
        }

        public void Start()
        {
            LoadGameProgress();

			_settingsMenuPresenter.Initialize();
			_levelsMenuPresenter.Initialize(_gameConfig.LevelConfigs, _gameProgress.LastAvailableLevelNumber);

            var levelStatisticsInfos = _gameProgress.LevelsChallengesResults
                .GroupJoin(_gameConfig.LevelConfigs, challengesResults => challengesResults.LevelId, levelConfig => levelConfig.Id, (challengesResults, levelConfigs) => (levelConfigs.First(), challengesResults))
                .ToArray();
            _statisticsMenuPresenter.Initialize(levelStatisticsInfos);

            PlayStartAnimationAsync().Forget();
        }

        private void LoadGameProgress()
        {
            _gameProgress = ES3.Load<GameProgress>(_gameConfig.GameProgressSaveKey);
            if (_gameProgress == null)
            {
                _gameProgress = new GameProgress()
                {
                    LastAvailableLevelNumber = 1,
                    LevelsChallengesResults = new LevelChallengesResults[_gameConfig.LevelConfigs.Length].Select(item => item = new LevelChallengesResults()).ToArray()
                };
                ES3.Save(_gameConfig.GameProgressSaveKey, _gameProgress);
            }
        }

        private void BindMainMenuSectionWithButton(IMainMenuSectionPresenter mainMenuSection, Button showSectionButton)
        {
            showSectionButton.onClick.AddListener(() => OnShowSectionButtonPressed(mainMenuSection).Forget());
            mainMenuSection.SectionDisabled += OnMainMenuSectionDisabled;
        }

        private async UniTask PlayStartAnimationAsync()
        {
            _mainMenuView.SetBlackScreenActive(true);
            await _mainMenuView.SetBlackScreenAlphaAsync(0f, 2f);
            _mainMenuView.SetBlackScreenActive(false);

            await SetMainMenuVisibilityAsync(true);
        }

        private async UniTask SetMainMenuVisibilityAsync(bool isVisible)
        {
            _mainMenuShowingTween ??= CreateMainMenuShowingTween();
            if (isVisible)
            {
                _mainMenuShowingTween.PlayForward();
            }
            else
            {
                _mainMenuShowingTween.PlayBackwards();
            }
            await _mainMenuShowingTween.AsyncWaitForRewind();
        }

        private Sequence CreateMainMenuShowingTween()
        {
            var backgroundParts = _mainMenuView.GetBackgroundParts();

            var backgroundFillingTotalDuration = 1.5f;
            var backgroundPartFillingDuration = backgroundFillingTotalDuration / backgroundParts.Length;
            _mainMenuView.SetContentLocalPosition(new Vector3(0, _mainMenuView.GetContentHeight(), 0));

            var tweenSequence = DOTween.Sequence();
            tweenSequence.Pause();
            tweenSequence.SetAutoKill(false);

            foreach (var backgroundPart in backgroundParts)
            {
                tweenSequence.Append(DOTween.To(x => backgroundPart.fillAmount = x, 0f, 1f, backgroundPartFillingDuration));
            }
            tweenSequence.Append(_mainMenuView.Content.transform.DOLocalMoveY(0f, 0.5f));
 
            return tweenSequence;
        }

        private async UniTaskVoid OnShowSectionButtonPressed(IMainMenuSectionPresenter mainMenuSection)
        {
            await SetMainMenuVisibilityAsync(false);
            await mainMenuSection.ShowSectionAsync();
        }

        private void OnMainMenuSectionDisabled() => SetMainMenuVisibilityAsync(true).Forget();

        private void OnExitButtonClicked() => Application.Quit();
    }
}