using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Scripts
{
    public class RewardingSectionAnimator : MonoBehaviour
    {
        public async UniTask ChangeVisibilityAsync(bool isVisible)
        {
            await transform
                .DOScale(isVisible ? 1f : 0f, 1.5f)
                .AsyncWaitForCompletion();
        }
    }
}
