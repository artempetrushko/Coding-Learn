using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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

        public IEnumerator PlayChallengeCompletingAnimation_COR()
        {
            starFillingImage.gameObject.SetActive(true);
            starFillingImage.transform.localScale = new Vector3(3f, 3f, 3f);

            var scalingTween = starFillingImage.transform.DOScale(1, 0.75f);
            yield return scalingTween.WaitForCompletion();
            challengeDescriptionText.color = Color.green;
        }
    }
}
