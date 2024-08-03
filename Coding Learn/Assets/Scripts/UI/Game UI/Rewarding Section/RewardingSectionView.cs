using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class RewardingSectionView : MonoBehaviour
    {
        [SerializeField] private Button _closeRewardingSectionButton;
        [SerializeField] private GameObject _challengeViewsContainer;
        
        public GameObject ChallengeViewsContainer => _challengeViewsContainer;

        public async UniTask SetVisibilityAsync(bool isVisible, float duration)
        {
            await transform
                .DOScale(isVisible ? 1f : 0f, duration)
                .AsyncWaitForCompletion();
        }

        public void SetCloseRewardingSectionButtonActive(bool isActive) => _closeRewardingSectionButton.gameObject.SetActive(isActive);
    }
}
