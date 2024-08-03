using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class LevelThumbnailView : MonoBehaviour
    {
        [SerializeField] private Image _thumbnail;

        public void SetThumbnail(Sprite thumbnail)
        {
            _thumbnail.sprite = thumbnail;
        }

        public async UniTask SetThumbnailFillAmountAsync(float fliiAmount, float duration)
        {
            await _thumbnail
                .DOFillAmount(fliiAmount, duration)
                .AsyncWaitForCompletion();
        }
    }
}
