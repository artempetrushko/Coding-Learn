using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Scripts
{
    public class ExitToMenuSectionView : MonoBehaviour
    {
        [SerializeField]
        private ExitToMenuSectionAnimator animator;
        
        public async UniTask ChangeVisibilityAsync(bool isVisible) => await animator.ChangeContentVisibilityAsync(isVisible);

        public async UniTask ShowBlackScreenAsync() => await animator.ShowBlackScreenAsync();
    }
}
