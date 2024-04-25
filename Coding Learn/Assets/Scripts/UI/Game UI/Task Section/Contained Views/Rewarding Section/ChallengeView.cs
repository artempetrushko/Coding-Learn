using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Scripts
{
    public class ChallengeView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text challengeDescriptionText;
        [SerializeField]
        private RewardingChallengeViewAnimator animator;

        public void SetChallengeDescription(string challengeDescription) => challengeDescriptionText.text = challengeDescription;

        public async UniTask PlayChallengeCompletedAnimationAsync() => await animator.PlayChallengeCompletingAnimationAsync();
    }
}
