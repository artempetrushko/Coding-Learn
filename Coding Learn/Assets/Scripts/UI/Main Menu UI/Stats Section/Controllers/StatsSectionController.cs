using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class StatsSectionController 
    {
        private StatsSectionView _statsSectionView;
        private LevelStatsCardView _levelStatsCardPrefab;
        private TaskStatsView _taskStatsPrefab;

        public StatsSectionController(StatsSectionView statsSectionView)
        {
            _statsSectionView = statsSectionView;
        }

        public async UniTask ChangeVisibilityAsync(bool isVisible)
        {
            _statsSectionView.SetBackToPreviousPageButtonActive(false);

            if (isVisible)
            {
                _statsSectionView.LevelStatsCardsContainer.transform.localPosition = Vector3.zero;
                _statsSectionView.DetalizedLevelStatsContainer.transform.localPosition = new Vector3(_statsSectionView.DetalizedLevelStatsContainer.GetComponent<RectTransform>().rect.width, 0, 0);
            }
            await _statsSectionView.transform
                .DOLocalMoveY(isVisible ? 0 : _statsSectionView.GetSectionHeight(), 0.75f)
                .AsyncWaitForCompletion();
        }

        public void CreateLevelStatsCards(List<LevelStatsCardData> cardDatas)
        {
            foreach (var cardData in cardDatas)
            {
                var levelCard = Object.Instantiate(_levelStatsCardPrefab, _statsSectionView.LevelStatsCardsContainer.transform);
                levelCard.SetLevelThumbnail(cardData.Thumbnail);
                levelCard.SetStarsCounterText($"{cardData.StarsCurrentCount}/{cardData.StarsTotalCount}");
                levelCard.ShowDetailedStatsButton.onClick.AddListener(cardData.CardPressedAction);
            }
        }

        public void ShowDetalizedLevelStats(List<TaskStatsData> taskStatsDatas)
        {
            CreateDetalizedLevelStats(taskStatsDatas);
            _statsSectionView.SetBackToPreviousPageButtonActive(true);
            ShowDetailedLevelStatsAsync().Forget();
        }

        public void ReturnToLevelStatsCards()
        {
            _statsSectionView.SetBackToPreviousPageButtonActive(false);
            ReturnToLevelCardsAsync().Forget();
        }

        private void CreateDetalizedLevelStats(List<TaskStatsData> taskStatsDatas)
        {
            DeletePreviousDetailedStats();
            foreach (var taskStatsData in taskStatsDatas)
            {
                var taskStats = Object.Instantiate(_taskStatsPrefab, _statsSectionView.DetalizedLevelStatsContainer.transform);
                taskStats.SetTaskTitleText(taskStatsData.TaskTitle);
                taskStats.SetStarsCounterText($"{taskStatsData.CompletedChallengesCount}/{taskStatsData.TotalChallengesCount}");
            }
        }

        private void DeletePreviousDetailedStats()
        {
            /*for (var i = detalizedLevelStats.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(detalizedLevelStats.transform.GetChild(i).gameObject);
            }
            detalizedLevelStats.transform.DetachChildren();*/
        }

        public async UniTask ShowDetailedLevelStatsAsync() => await ShowNewStatsContentAsync(_statsSectionView.LevelStatsCardsContainer, _statsSectionView.DetalizedLevelStatsContainer, -1);

        public async UniTask ReturnToLevelCardsAsync() => await ShowNewStatsContentAsync(_statsSectionView.DetalizedLevelStatsContainer, _statsSectionView.LevelStatsCardsContainer, 1);

        private async UniTask ShowNewStatsContentAsync(GameObject previousStatsContent, GameObject newStatsContent, int movementSign)
        {
            var tweenSequence = DOTween.Sequence();
            tweenSequence
                .Append(previousStatsContent.transform.DOLocalMoveX(previousStatsContent.GetComponent<RectTransform>().rect.width * movementSign, 0.75f))
                .Append(newStatsContent.transform.DOLocalMoveX(0, 0.75f));
            await tweenSequence.Play().AsyncWaitForCompletion();
        }





        private const float FOREGROUND_END_ALPHA = 0.8f;
        private const float VISIBILITY_CHANGE_DURATION = 0.2f;

        /*public async UniTask ShowStarsCounter() => await ChangeStarsCounterVisibilityAsync(0f, FOREGROUND_END_ALPHA);

        public async UniTask HideStarsCounter() => await ChangeStarsCounterVisibilityAsync(_starsCounter.transform.localPosition.y, 0f);

        private async UniTask ChangeStarsCounterVisibilityAsync(float counterEndPositionY, float foregroundEndAlpha)
        {
            _foreground.DOFade(foregroundEndAlpha, VISIBILITY_CHANGE_DURATION);
            _starsCounter.transform.DOLocalMoveY(counterEndPositionY, VISIBILITY_CHANGE_DURATION);
            await UniTask.WaitForSeconds(VISIBILITY_CHANGE_DURATION);
        }*/

    }
}
