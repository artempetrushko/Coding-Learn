using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameLogic;
using SaveSystem;
using UI.MainMenu;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace MainMenu
{
    public class StatisticsMenuPresenter : IMainMenuSectionPresenter
    {
        public event Action SectionDisabled;

        private const float MENU_VISIBILITY_CHANGING_DURATION = 0.75f;
        private const float LEVEL_STATS_CARD_VISIBILITY_CHANGE_DURATION = 0.2f;

        private StatisticsMenuModel _statisticsMenuModel;
        private StatisticsMenuView _statisticsMenuView;
        private LevelStatisticsCardView _levelStatsCardPrefab;
        private TaskStatisticsPageView _taskStatisticsPageViewPrefab;
        private TaskStatisticsView _taskStatisticsViewPrefab;

        public StatisticsMenuPresenter(StatisticsMenuView statsSectionView, LevelStatisticsCardView levelStatsCardViewPrefab, TaskStatisticsPageView taskStatisticsPageViewPrefab, TaskStatisticsView taskStatsViewPrefab)
        {
            _statisticsMenuView = statsSectionView;
            _levelStatsCardPrefab = levelStatsCardViewPrefab;
            _taskStatisticsPageViewPrefab = taskStatisticsPageViewPrefab;
            _taskStatisticsViewPrefab = taskStatsViewPrefab;

            _statisticsMenuView.CloseViewButton.onClick.AddListener(OnCloseViewButtonPressed);
            _statisticsMenuView.BackToPreviousPageButton.onClick.AddListener(OnBackToPreviousPageButtonPressed);
        }

        public async UniTask ShowSectionAsync()
        {
            _statisticsMenuView.SetActive(true);
            await SetSectionVisibilityAsync(true);
        }

        public async UniTask HideSectionAsync()
        {
            await SetSectionVisibilityAsync(false);
            _statisticsMenuView.SetActive(false);
        }

        public void Initialize((LevelConfig levelConfig, LevelChallengesResults levelChallengesResults)[] levelStatisticsInfos)
        {
            CreateStatisticsViewsAsync(levelStatisticsInfos).Forget();
        }

        private async UniTask SetSectionVisibilityAsync(bool isVisible)
        {
            _statisticsMenuView.SetBackToPreviousPageButtonActive(false);

            if (isVisible)
            {
                _statisticsMenuView.LevelStatisticsCardsContainer.transform.localPosition = Vector3.zero;
                _statisticsMenuView.DetailedLevelStatisticsPagesContainer.transform.localPosition = new Vector3(_statisticsMenuView.DetailedLevelStatisticsPagesContainer.GetComponent<RectTransform>().rect.width, 0, 0);
            }
            await _statisticsMenuView.transform
                .DOLocalMoveY(isVisible ? 0 : _statisticsMenuView.GetSectionHeight(), MENU_VISIBILITY_CHANGING_DURATION)
                .AsyncWaitForCompletion();
        }

        private async UniTask CreateStatisticsViewsAsync((LevelConfig levelConfig, LevelChallengesResults levelChallengesResults)[] levelStatisticsInfos)
        {
            var levelStatisticsCardDatas = new List<(LevelStatisticsCardView levelStatisticsCard, TaskStatisticsPageView linkedStatisticsPage)>();

            foreach (var levelStatisticsInfo in levelStatisticsInfos)
            {
                var totalChallengesCount = levelStatisticsInfo.levelConfig.Content.Quests.Sum(quest => quest.Task.ChallengesConfig.Challenges.Length);
                var completedChallengesCount = levelStatisticsInfo.levelChallengesResults.TasksChallengesResults != null && levelStatisticsInfo.levelChallengesResults.TasksChallengesResults.Length > 0
                    ? levelStatisticsInfo.levelChallengesResults.TasksChallengesResults.Sum(taskChallengesResults => taskChallengesResults.ChallengeResults.Count(challengeResult => challengeResult.IsCompleted))
                    : 0;

                var levelStatisticsCard = Object.Instantiate(_levelStatsCardPrefab, _statisticsMenuView.LevelStatisticsCardsContainer.transform);

                var levelThumbnail = await Addressables.LoadAssetAsync<Sprite>(levelStatisticsInfo.levelConfig.ThumbnailReference);
                levelStatisticsCard.SetLevelThumbnail(levelThumbnail);
                levelStatisticsCard.SetStarsCounterText($"{completedChallengesCount}/{totalChallengesCount}");
                levelStatisticsCard.ShowDetailedStatisticsButton.onClick.AddListener(() => OnShowDetailedStatisticsButtonPressed(levelStatisticsCard));
                levelStatisticsCard.PointerEnter += OnLevelCardPointerEnter;
                levelStatisticsCard.PointerExit += OnLevelCardPointerExit;

                var taskStatisticsPage = Object.Instantiate(_taskStatisticsPageViewPrefab, _statisticsMenuView.DetailedLevelStatisticsPagesContainer.transform);
                foreach (var taskChallengesResults in levelStatisticsInfo.levelChallengesResults.TasksChallengesResults)
                {
                    var taskStatisticsView = Object.Instantiate(_taskStatisticsViewPrefab, taskStatisticsPage.transform);

                    var task = levelStatisticsInfo.levelConfig.Content.Quests.First(quest => quest.Task.Id == taskChallengesResults.TaskId).Task;
                    var taskTitle = task.Title.GetLocalizedString();
                    taskStatisticsView.SetTaskTitleText(taskTitle);

                    var totalTaskChallengesCount = taskChallengesResults.ChallengeResults.Count(challengeResult => challengeResult.IsCompleted);
                    var completedTaskChallengesCount = task.ChallengesConfig.Challenges.Length;
                    taskStatisticsView.SetStarsCounterText($"{completedTaskChallengesCount}/{totalTaskChallengesCount}");
                }

                levelStatisticsCardDatas.Add((levelStatisticsCard, taskStatisticsPage));
            }

            _statisticsMenuModel = new StatisticsMenuModel(levelStatisticsCardDatas.ToArray());
        }

        private async UniTask ShowDetailedLevelStatsAsync(LevelStatisticsCardView selectedLevelStatisticsCardView)
        {
            _statisticsMenuModel.SelectedStatisticsPage = _statisticsMenuModel.LevelStatisticsCardDatas.First(data => data.levelStatisticsCard == selectedLevelStatisticsCardView).linkedStatisticsPage;
            _statisticsMenuModel.SelectedStatisticsPage.SetActive(true);

            _statisticsMenuView.SetBackToPreviousPageButtonActive(true);
            await ShowNewStatsContentAsync(_statisticsMenuView.LevelStatisticsCardsContainer, _statisticsMenuView.DetailedLevelStatisticsPagesContainer, -1);
        }

        private async UniTask ReturnToLevelCardsAsync()
        {
            _statisticsMenuView.SetBackToPreviousPageButtonActive(false);
            await ShowNewStatsContentAsync(_statisticsMenuView.DetailedLevelStatisticsPagesContainer, _statisticsMenuView.LevelStatisticsCardsContainer, 1);

            _statisticsMenuModel.SelectedStatisticsPage.SetActive(false);
            _statisticsMenuModel.SelectedStatisticsPage = null;
        }

        private async UniTask ShowNewStatsContentAsync(GameObject previousStatsContent, GameObject newStatsContent, int movementSign)
        {
            await previousStatsContent.transform.DOLocalMoveX(previousStatsContent.GetComponent<RectTransform>().rect.width * movementSign, 0.75f).AsyncWaitForCompletion();
            await newStatsContent.transform.DOLocalMoveX(0, 0.75f).AsyncWaitForCompletion();
        }

        private async UniTask SetLevelStatsCardStarsCounterVisibilityAsync(LevelStatisticsCardView levelStatsCardView, float counterEndPositionY, float foregroundEndAlpha)
        {
            levelStatsCardView.Foreground.DOFade(foregroundEndAlpha, LEVEL_STATS_CARD_VISIBILITY_CHANGE_DURATION).ToUniTask().Forget();
            levelStatsCardView.StarsCounter.transform.DOLocalMoveY(counterEndPositionY, LEVEL_STATS_CARD_VISIBILITY_CHANGE_DURATION).ToUniTask().Forget();
            await UniTask.WaitForSeconds(LEVEL_STATS_CARD_VISIBILITY_CHANGE_DURATION);
        }

        private void OnCloseViewButtonPressed() => HideSectionAsync().Forget();

        private void OnShowDetailedStatisticsButtonPressed(LevelStatisticsCardView levelStatisticsCardView) => ShowDetailedLevelStatsAsync(levelStatisticsCardView).Forget();

        private void OnBackToPreviousPageButtonPressed() => ReturnToLevelCardsAsync().Forget();

        private void OnLevelCardPointerEnter(LevelStatisticsCardView levelStatsCardView) => SetLevelStatsCardStarsCounterVisibilityAsync(levelStatsCardView, 0f, 0.8f).Forget();

        private void OnLevelCardPointerExit(LevelStatisticsCardView levelStatsCardView) => SetLevelStatsCardStarsCounterVisibilityAsync(levelStatsCardView, levelStatsCardView.StarsCounter.transform.localPosition.y, 0f).Forget();
    }
}
