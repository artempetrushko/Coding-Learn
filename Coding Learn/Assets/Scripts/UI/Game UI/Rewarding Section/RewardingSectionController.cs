using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace Scripts
{
    public class RewardingSectionController
    {
        private const float VISIBILITY_CHANGING_DURATION = 1.5f;

        private RewardingSectionView _rewardingSectionView;
        private AssetReference _challengeViewPrefab;
        private DiContainer _diContainer;

        public RewardingSectionController(DiContainer diContainer, RewardingSectionView rewardingSectionView, AssetReference challengeViewPrefab)
        {
            _rewardingSectionView = rewardingSectionView;
            _challengeViewPrefab = challengeViewPrefab;
            _diContainer = diContainer;
        }

        public async UniTask ShowChallengesResultsAsync((string description, bool isCompleted)[] challengeDatas)
        {
            ClearChallengeViews();

            await _rewardingSectionView.SetVisibilityAsync(true, VISIBILITY_CHANGING_DURATION);
            foreach (var challengeData in challengeDatas)
            {
                var challengeViewLoadingTask = _challengeViewPrefab.LoadAssetAsync<ChallengeView>();
                await challengeViewLoadingTask.Task;
                if (challengeViewLoadingTask.Status == AsyncOperationStatus.Succeeded)
                {
                    var challengeView = _diContainer.InstantiatePrefab(challengeViewLoadingTask.Result, _rewardingSectionView.ChallengeViewsContainer.transform).GetComponent<ChallengeView>();
                    challengeView.SetChallengeDescription(challengeData.description);
                    if (challengeData.isCompleted)
                    {
                        await challengeView.PlayChallengeCompletingAnimationAsync();
                    }
                    await UniTask.WaitForSeconds(0.5f);
                }
            }

            _rewardingSectionView.SetCloseRewardingSectionButtonActive(true);
        }

        public async UniTask HideChallengesResultsAsync() => await _rewardingSectionView.SetVisibilityAsync(false, VISIBILITY_CHANGING_DURATION);

        private void ClearChallengeViews()
        {
            for (var i = _rewardingSectionView.ChallengeViewsContainer.transform.childCount - 1; i >= 0; i--)
            {
                Object.Destroy(_rewardingSectionView.ChallengeViewsContainer.transform.GetChild(i).gameObject);
            }
            _rewardingSectionView.ChallengeViewsContainer.transform.DetachChildren();
        }
    }
}
