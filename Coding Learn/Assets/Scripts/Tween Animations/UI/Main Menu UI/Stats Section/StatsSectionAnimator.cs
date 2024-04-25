using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Scripts
{
    public class StatsSectionAnimator : MonoBehaviour
    {
        [SerializeField]
        private GameObject levelCardsContainer;
        [SerializeField]
        private GameObject detailedLevelStatsContainer;

        public async UniTask ChangeVisibilityAsync(bool isVisible)
        {
            if (isVisible)
            {
                levelCardsContainer.transform.localPosition = Vector3.zero;
                detailedLevelStatsContainer.transform.localPosition = new Vector3(detailedLevelStatsContainer.GetComponent<RectTransform>().rect.width, 0, 0);
            }
            await transform
                .DOLocalMoveY(isVisible ? 0 : GetComponent<RectTransform>().rect.height, 0.75f)
                .AsyncWaitForCompletion();
        }

        public async UniTask ShowDetailedLevelStatsAsync() => await ShowNewStatsContentAsync(levelCardsContainer, detailedLevelStatsContainer, -1);

        public async UniTask ReturnToLevelCardsAsync() => await ShowNewStatsContentAsync(detailedLevelStatsContainer, levelCardsContainer, 1);

        private async UniTask ShowNewStatsContentAsync(GameObject previousStatsContent, GameObject newStatsContent, int movementSign)
        {
            var tweenSequence = DOTween.Sequence();
            tweenSequence
                .Append(previousStatsContent.transform.DOLocalMoveX(previousStatsContent.GetComponent<RectTransform>().rect.width * movementSign, 0.75f))
                .Append(newStatsContent.transform.DOLocalMoveX(0, 0.75f));
            await tweenSequence.Play().AsyncWaitForCompletion();
        }
    }
}
