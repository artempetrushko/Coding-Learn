using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class RewardingSectionView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text headerText;
        [SerializeField] 
        private Button closeRewardingSectionButton;
        [Space, SerializeField]
        private GameObject challengeViewsContainer;
        [SerializeField]
        private ChallengeView challengeViewPrefab;
        [Space, SerializeField]
        private RewardingSectionAnimator animator;

        public IEnumerator ShowChallengesResults_COR(List<(string description, bool isCompleted)> challengeDatas)
        {
            ClearChallengeViews();

            yield return StartCoroutine(animator.ChangeVisibility_COR(true));
            foreach (var challengeData in challengeDatas)
            {
                var challengeView = Instantiate(challengeViewPrefab, challengeViewsContainer.transform);
                challengeView.SetChallengeDescription(challengeData.description);
                if (challengeData.isCompleted)
                {
                    yield return StartCoroutine(challengeView.PlayChallengeCompletedAnimation_COR());
                }
                yield return new WaitForSeconds(0.5f);
            }
            closeRewardingSectionButton.gameObject.SetActive(true);
        }

        public IEnumerator HideChallengesResults_COR()
        {
            yield return StartCoroutine(animator.ChangeVisibility_COR(false));
        }

        private void ClearChallengeViews()
        {
            for (var i = challengeViewsContainer.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(challengeViewsContainer.transform.GetChild(i).gameObject);
            }
            challengeViewsContainer.transform.DetachChildren();
        }
    }
}
