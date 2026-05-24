using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameLogic;
using MainMenu;
using SaveSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace UI.MainMenu
{
    public class StatisticsMenuView : MonoBehaviour
    {
        [SerializeField] private Button _backToPreviousPageButton;
        [SerializeField] private Button _closeViewButton;
        [SerializeField] private GameObject _levelStatisticsCardsContainer;
        [SerializeField] private GameObject _detailedLevelStatisticsPagesContainer;

        public event Action SectionDisabled;

        private const float MENU_VISIBILITY_CHANGING_DURATION = 0.75f;
        private const float LEVEL_STATS_CARD_VISIBILITY_CHANGE_DURATION = 0.2f;

        private StatisticsMenuModel _statisticsMenuModel;
        private LevelStatisticsCardView _levelStatsCardPrefab;
        private TaskStatisticsPageView _taskStatisticsPageViewPrefab;
        private TaskStatisticsView _taskStatisticsViewPrefab;

        public StatisticsMenuView(StatisticsMenuView statsSectionView, LevelStatisticsCardView levelStatsCardViewPrefab, TaskStatisticsPageView taskStatisticsPageViewPrefab, TaskStatisticsView taskStatsViewPrefab)
        {
            _levelStatsCardPrefab = levelStatsCardViewPrefab;
            _taskStatisticsPageViewPrefab = taskStatisticsPageViewPrefab;
            _taskStatisticsViewPrefab = taskStatsViewPrefab;

            _closeViewButton.onClick.AddListener(OnCloseViewButtonPressed);
            _backToPreviousPageButton.onClick.AddListener(OnBackToPreviousPageButtonPressed);
        }

        public async UniTask ShowSectionAsync()
        {
            gameObject.SetActive(true);
            await SetSectionVisibilityAsync(true);
        }

        public async UniTask HideSectionAsync()
        {
            await SetSectionVisibilityAsync(false);
            gameObject.SetActive(false);
        }

        public void Initialize((LevelConfig levelConfig, LevelChallengesResults levelChallengesResults)[] levelStatisticsInfos)
        {
            CreateStatisticsViewsAsync(levelStatisticsInfos).Forget();
        }

        private async UniTask SetSectionVisibilityAsync(bool isVisible)
        {
            _backToPreviousPageButton.gameObject.SetActive(false);

            if (isVisible)
            {
                _levelStatisticsCardsContainer.transform.localPosition = Vector3.zero;
                _detailedLevelStatisticsPagesContainer.transform.localPosition = new Vector3(_detailedLevelStatisticsPagesContainer.GetComponent<RectTransform>().rect.width, 0, 0);
            }
            await transform
                .DOLocalMoveY(isVisible ? 0 : GetComponent<RectTransform>().rect.height, MENU_VISIBILITY_CHANGING_DURATION)
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

                var levelStatisticsCard = Object.Instantiate(_levelStatsCardPrefab, _levelStatisticsCardsContainer.transform);

                var levelThumbnail = await Addressables.LoadAssetAsync<Sprite>(levelStatisticsInfo.levelConfig.ThumbnailReference);
                levelStatisticsCard.SetLevelThumbnail(levelThumbnail);
                levelStatisticsCard.SetStarsCounterText($"{completedChallengesCount}/{totalChallengesCount}");
                levelStatisticsCard.ShowDetailedStatisticsButton.onClick.AddListener(() => OnShowDetailedStatisticsButtonPressed(levelStatisticsCard));
                levelStatisticsCard.PointerEnter += OnLevelCardPointerEnter;
                levelStatisticsCard.PointerExit += OnLevelCardPointerExit;

                var taskStatisticsPage = Object.Instantiate(_taskStatisticsPageViewPrefab, _detailedLevelStatisticsPagesContainer.transform);
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

            _backToPreviousPageButton.gameObject.SetActive(true);
            await ShowNewStatsContentAsync(_levelStatisticsCardsContainer, _detailedLevelStatisticsPagesContainer, -1);
        }

        private async UniTask ReturnToLevelCardsAsync()
        {
            _backToPreviousPageButton.gameObject.SetActive(false);
            await ShowNewStatsContentAsync(_detailedLevelStatisticsPagesContainer, _levelStatisticsCardsContainer, 1);

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
