using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Scripts
{
    public class ChallengeView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text challengeDescriptionText;
        [SerializeField]
        private RewardingChallengeViewAnimator animator;

        public void SetChallengeDescription(string challengeDescription) => challengeDescriptionText.text = challengeDescription;

        public IEnumerator PlayChallengeCompletedAnimation_COR()
        {
            yield return StartCoroutine(animator.PlayChallengeCompletingAnimation_COR());
        }
    }
}
