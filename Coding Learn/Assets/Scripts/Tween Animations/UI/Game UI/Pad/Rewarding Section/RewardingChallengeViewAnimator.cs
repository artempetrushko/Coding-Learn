using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class RewardingChallengeViewAnimator : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text challengeDescriptionText;
        [SerializeField]
        private Image starFillingImage;

        public async UniTask PlayChallengeCompletingAnimationAsync()
        {
            starFillingImage.gameObject.SetActive(true);
            starFillingImage.transform.localScale = new Vector3(3f, 3f, 3f);

            await starFillingImage.transform
                .DOScale(1, 0.75f)
                .AsyncWaitForCompletion();
            challengeDescriptionText.color = Color.green;
        }
    }
}
