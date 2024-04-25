using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Scripts
{
    public class SettingsSectionAnimator : MonoBehaviour
    {
        public async UniTask ChangeVisibilityAsync(bool isVisible)
        {
            await transform
                .DOLocalMoveY(isVisible ? 0 : GetComponent<RectTransform>().rect.height, 0.75f)
                .AsyncWaitForCompletion();
        }
    }
}
