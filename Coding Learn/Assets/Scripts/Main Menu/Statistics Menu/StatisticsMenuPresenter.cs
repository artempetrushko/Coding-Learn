using System;
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
    public class StatisticsMenuModel
    {

    }

    public class StatisticsMenuPresenter : IMainMenuSectionPresenter
    {
        public event Action SectionDisabled;

        private const float SECTION_VISIBILITY_CHANGING_DURATION = 0.75f;

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
                .DOLocalMoveY(isVisible ? 0 : _statisticsMenuView.GetSectionHeight(), SECTION_VISIBILITY_CHANGING_DURATION)
                .AsyncWaitForCompletion();
        }

        private async UniTask CreateStatisticsViewsAsync((LevelConfig levelConfig, LevelChallengesResults levelChallengesResults)[] levelStatisticsInfos)
        {
            foreach (var levelStatisticsInfo in levelStatisticsInfos)
            {
                var totalChallengesCount = levelStatisticsInfo.levelConfig.Content.Quests.Sum(quest => quest.Task.ChallengesConfig.Challenges.Length);
                var completedChallengesCount = levelStatisticsInfo.levelChallengesResults.TasksChallengesResults != null && levelStatisticsInfo.levelChallengesResults.TasksChallengesResults.Length > 0
                    ? levelStatisticsInfo.levelChallengesResults.TasksChallengesResults.Sum(taskChallengesResults => taskChallengesResults.ChallengeResults.Count(challengeResult => challengeResult.IsCompleted))
                    : 0;

                var levelCard = Object.Instantiate(_levelStatsCardPrefab, _statisticsMenuView.LevelStatisticsCardsContainer.transform);

                var levelThumbnail = await Addressables.LoadAssetAsync<Sprite>(levelStatisticsInfo.levelConfig.LoadingScreenReference);
                levelCard.SetLevelThumbnail(levelThumbnail);


                levelCard.SetStarsCounterText($"{completedChallengesCount}/{totalChallengesCount}");

                levelCard.ShowDetailedStatisticsButton.onClick.AddListener(OnShowDetailedStatisticsButtonPressed);
                levelCard.PointerEnter += OnLevelCardPointerEnter;
                levelCard.PointerExit += OnLevelCardPointerExit;




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


            }
        }




        

        public async UniTask ShowDetailedLevelStatsAsync() => await ShowNewStatsContentAsync(_statisticsMenuView.LevelStatisticsCardsContainer, _statisticsMenuView.DetailedLevelStatisticsPagesContainer, -1);

        public async UniTask ReturnToLevelCardsAsync() => await ShowNewStatsContentAsync(_statisticsMenuView.DetailedLevelStatisticsPagesContainer, _statisticsMenuView.LevelStatisticsCardsContainer, 1);

        private async UniTask ShowNewStatsContentAsync(GameObject previousStatsContent, GameObject newStatsContent, int movementSign)
        {
            await previousStatsContent.transform.DOLocalMoveX(previousStatsContent.GetComponent<RectTransform>().rect.width * movementSign, 0.75f).AsyncWaitForCompletion();
            await newStatsContent.transform.DOLocalMoveX(0, 0.75f).AsyncWaitForCompletion();
        }



        private const float FOREGROUND_END_ALPHA = 0.8f;
        private const float VISIBILITY_CHANGE_DURATION = 0.2f;

        private async UniTask SetLevelStatsCardStarsCounterVisibilityAsync(LevelStatisticsCardView levelStatsCardView, float counterEndPositionY, float foregroundEndAlpha)
        {
            levelStatsCardView.Foreground.DOFade(foregroundEndAlpha, VISIBILITY_CHANGE_DURATION).ToUniTask().Forget();
            levelStatsCardView.StarsCounter.transform.DOLocalMoveY(counterEndPositionY, VISIBILITY_CHANGE_DURATION).ToUniTask().Forget();
            await UniTask.WaitForSeconds(VISIBILITY_CHANGE_DURATION);
        }

        private void OnShowDetailedStatisticsButtonPressed()
        {
            //CreateDetalizedLevelStats(taskStatsDatas);
            _statisticsMenuView.SetBackToPreviousPageButtonActive(true);
            ShowDetailedLevelStatsAsync().Forget();
        }

        private void OnBackToPreviousPageButtonPressed()
        {
            _statisticsMenuView.SetBackToPreviousPageButtonActive(false);
            ReturnToLevelCardsAsync().Forget();
        }

        private void OnLevelCardPointerEnter(LevelStatisticsCardView levelStatsCardView) => SetLevelStatsCardStarsCounterVisibilityAsync(levelStatsCardView, 0f, FOREGROUND_END_ALPHA).Forget();

        private void OnLevelCardPointerExit(LevelStatisticsCardView levelStatsCardView) => SetLevelStatsCardStarsCounterVisibilityAsync(levelStatsCardView, levelStatsCardView.StarsCounter.transform.localPosition.y, 0f).Forget();
    }
}
