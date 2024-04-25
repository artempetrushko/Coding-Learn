using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
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
        private ChallengeView challengeViewPrefab;
        [Space, SerializeField]
        private RewardingSectionAnimator animator;

        public async UniTask ShowChallengesResultsAsync(List<(string description, bool isCompleted)> challengeDatas)
        {
            ClearChallengeViews();

            await animator.ChangeVisibilityAsync(true);
            foreach (var challengeData in challengeDatas)
            {
                var challengeView = Instantiate(challengeViewPrefab, challengeViewsContainer.transform);
                challengeView.SetChallengeDescription(challengeData.description);
                if (challengeData.isCompleted)
                {
                    await challengeView.PlayChallengeCompletedAnimationAsync();
                }
                await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
            }
            closeRewardingSectionButton.gameObject.SetActive(true);
        }

        public async UniTask HideChallengesResultsAsync() => await animator.ChangeVisibilityAsync(false);

        private void ClearChallengeViews()
        {
            for (var i = challengeViewsContainer.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(challengeViewsContainer.transform.GetChild(i).gameObject);
            }
            challengeViewsContainer.transform.DetachChildren();
        }
    }
}
