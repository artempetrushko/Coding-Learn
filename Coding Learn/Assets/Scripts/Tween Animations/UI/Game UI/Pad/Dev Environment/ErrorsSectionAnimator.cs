using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Scripts
{
    public class ErrorsSectionAnimator : MonoBehaviour
    {
        public async UniTask ChangeVisibilityAsync(bool isVisible)
        {
            var movementSign = isVisible ? 1 : -1;
            await transform
                .DOLocalMoveY(transform.localPosition.y + transform.GetComponent<RectTransform>().sizeDelta.y * movementSign, 1.5f)
                .AsyncWaitForCompletion();
        }
    }
}
