using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class ChallengeView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text challengeDescriptionText;

        public void SetChallengeDescription(string challengeDescription) => challengeDescriptionText.text = challengeDescription;

        public async UniTask PlayChallengeCompletedAnimationAsync() => await PlayChallengeCompletingAnimationAsync();


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
