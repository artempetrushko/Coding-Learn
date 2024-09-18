using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using SaveSystem;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameLogic
{
    public class ChallengesPresenter : IPadModalSection
    {
        public event Action ChallengesCompletingChecked;

        private const float VISIBILITY_CHANGING_DURATION = 1.5f;
        private const float CHALLENGE_VIEWS_SHOWING_INTERVAL = 0.5f;
        private const float CHALLENGE_VIEW_STAR_IMAGE_START_SCALE = 3f;

        private readonly Color COMPLETED_CHALLENGE_DESCRIPTION_COLOR = Color.green;

        private ChallengesView _challengesView;
        private ChallengeView _challengeViewPrefab;
        private ChallengesRewardingView _challengesRewardingView;
        private ChallengeRewardView _challengeRewardViewPrefab;

        private ChallengesConfig currentChallengesData;
        private TaskChallengesResults currentTaskChallengesResults;

        public ChallengesPresenter(ChallengesView challengesView, ChallengeView challengeViewPrefab, ChallengesRewardingView challengesRewardingView, ChallengeRewardView challengeRewardViewPrefab)
        {
            _challengesView = challengesView;
            _challengeViewPrefab = challengeViewPrefab;
            _challengesRewardingView = challengesRewardingView;
            _challengeRewardViewPrefab = challengeRewardViewPrefab;

            _challengesView.CloseViewButton.onClick.AddListener(() => HideModalSectionAsync().Forget());
            _challengesRewardingView.CloseViewButton.onClick.AddListener(() => OnChallengesRewardingViewClosedAsync().Forget());
        }

        public async UniTask ShowModalSectionAsync()
        {
            _challengesView.SetActive(true);
            await _challengesView.CanvasGroup.DOFade(1f, VISIBILITY_CHANGING_DURATION).AsyncWaitForCompletion();
        }

        public async UniTask HideModalSectionAsync()
        {
            await _challengesView.CanvasGroup.DOFade(0f, VISIBILITY_CHANGING_DURATION).AsyncWaitForCompletion();
            _challengesView.SetActive(false);           
        }

        public void SetChallengeData(ChallengesConfig challengesData)
        {
            currentChallengesData = challengesData;
            var challengeDescriptions = currentChallengesData.Challenges.Select(challenge => challenge.Description.GetLocalizedString()).ToList();
            CreateNewChallengeViews(challengeDescriptions);

            //currentTaskChallengesResults = GameSaveManager.GetCurrentTaskChallengesResults(currentTaskNumber);
            //currentTaskChallengesResults.ChallengeResults ??= new bool[currentChallengesData.Challenges.Length].ToList();
        }

        public void CheckChallengesCompleting(CodingTaskModel codingTaskModel, bool isTaskSkipped)
        {
            var challengeDatas = currentChallengesData.Challenges
                                    .Select(challenge => (description: challenge.Description, isCompleted: !isTaskSkipped && challenge.Checker.IsCompleted(codingTaskModel)))
                                    .ToArray();
            for (var i = 0; i < currentTaskChallengesResults.ChallengeResults.Length; i++)
            {
                if (challengeDatas[i].isCompleted && !currentTaskChallengesResults.ChallengeResults[i].IsCompleted)
                {
                    currentTaskChallengesResults.ChallengeResults[i].IsCompleted = challengeDatas[i].isCompleted;
                }
            }
        }

        public async UniTask ShowChallengesResultsAsync((string description, bool isCompleted)[] challengeDatas)
        {
            await SetChallengesRewardingVIewVisibilityAsync(true);

            foreach (var challengeData in challengeDatas)
            {
                var challengeView = Object.Instantiate(_challengeRewardViewPrefab, _challengesRewardingView.ChallengeViewsContainer.transform);
                challengeView.SetChallengeDescription(challengeData.description);
                if (challengeData.isCompleted)
                {
                    challengeView.StarFillingImage.gameObject.SetActive(true);
                    challengeView.StarFillingImage.transform.localScale = new Vector3(CHALLENGE_VIEW_STAR_IMAGE_START_SCALE, CHALLENGE_VIEW_STAR_IMAGE_START_SCALE, CHALLENGE_VIEW_STAR_IMAGE_START_SCALE);
                    await challengeView.StarFillingImage.transform
                        .DOScale(1, 0.75f)
                        .AsyncWaitForCompletion();
                    challengeView.SetChallengeDescriptionColor(COMPLETED_CHALLENGE_DESCRIPTION_COLOR);
                }
                await UniTask.WaitForSeconds(CHALLENGE_VIEWS_SHOWING_INTERVAL);
            }

            _challengesRewardingView.CloseViewButton.gameObject.SetActive(true);
        }

        

        

        public void CreateNewChallengeViews(List<string> challengeDescriptions)
        {
            foreach (string description in challengeDescriptions)
            {
                var challengeView = Object.Instantiate(_challengeViewPrefab, _challengesView.ChallengesContainer.transform);
                challengeView.SetDescriptionText(description);
            }
        }

        private async UniTask SetChallengesRewardingVIewVisibilityAsync(bool isVisible)
        {
            await _challengesRewardingView.transform
                .DOScale(isVisible ? 1f : 0f, VISIBILITY_CHANGING_DURATION)
                .AsyncWaitForCompletion();
        }

        private async UniTask OnChallengesRewardingViewClosedAsync()
        {
            await SetChallengesRewardingVIewVisibilityAsync(false);

            _challengesView.ChallengesContainer.transform.DeleteAllChildren();
            _challengesRewardingView.ChallengeViewsContainer.transform.DeleteAllChildren();

            ChallengesCompletingChecked?.Invoke();
        }
    }
}
