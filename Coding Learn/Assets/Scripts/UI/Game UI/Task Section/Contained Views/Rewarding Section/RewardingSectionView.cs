using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace Scripts
{
    public class RewardingSectionView : MonoBehaviour
    {
        [SerializeField] 
        private Button closeRewardingSectionButton;
        [Space, SerializeField]
        private GameObject challengeViewsContainer;
        [SerializeField]
        private AssetReference challengeViewPrefab;

        public async UniTask ShowChallengesResultsAsync((string description, bool isCompleted)[] challengeDatas)
        {
            ClearChallengeViews();

            await ChangeVisibilityAsync(true);
            foreach (var challengeData in challengeDatas)
            {
                var challengeViewLoadingTask = challengeViewPrefab.LoadAssetAsync<ChallengeView>();
                await challengeViewLoadingTask.Task;
                if (challengeViewLoadingTask.Status == AsyncOperationStatus.Succeeded)
                {
                    var challengeView = Instantiate(challengeViewLoadingTask.Result, challengeViewsContainer.transform);
                    challengeView.SetChallengeDescription(challengeData.description);
                    if (challengeData.isCompleted)
                    {
                        await challengeView.PlayChallengeCompletedAnimationAsync();
                    }
                    await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
                }  
            }
            closeRewardingSectionButton.gameObject.SetActive(true);
        }

        public async UniTask HideChallengesResultsAsync() => await ChangeVisibilityAsync(false);

        private void ClearChallengeViews()
        {
            for (var i = challengeViewsContainer.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(challengeViewsContainer.transform.GetChild(i).gameObject);
            }
            challengeViewsContainer.transform.DetachChildren();
        }



        public async UniTask ChangeVisibilityAsync(bool isVisible)
        {
            await transform
                .DOScale(isVisible ? 1f : 0f, 1.5f)
                .AsyncWaitForCompletion();
        }
    }
}
