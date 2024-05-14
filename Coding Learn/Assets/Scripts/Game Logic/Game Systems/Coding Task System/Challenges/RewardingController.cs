using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Scripts
{
    public class RewardingController
    {
        private RewardingSectionView rewardingSectionView;

        public async UniTask ShowChallengesResults((string description, bool isCompleted)[] challengeResults) => await rewardingSectionView.ShowChallengesResultsAsync(challengeResults);  

        public void HideChallengesResults()
        {
            UniTask.Void(async () =>
            {
                await rewardingSectionView.HideChallengesResultsAsync();
                //OnChallengesCompletingChecked?.Invoke();
            });
        }
    }
}
