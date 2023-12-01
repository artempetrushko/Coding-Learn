using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class PadChallengesScreenView : MonoBehaviour
    {
        [SerializeField]
        private PadChallengeView challengeViewPrefab;
        [SerializeField]
        private GameObject challengesContainer;
        [Space, SerializeField]
        private PadViewsAnimator animator;

        public void ChangeVisibility(bool isVisible) => StartCoroutine(animator.ChangeViewVisibility_COR(gameObject, isVisible));

        public void CreateNewChallengeViews(List<string> challengeDescriptions)
        {
            DeletePreviousChallenges();
            foreach (string description in challengeDescriptions)
            {
                var challengeView = Instantiate(challengeViewPrefab, challengesContainer.transform);
                challengeView.SetInfo(description);
            }
        }

        private void DeletePreviousChallenges()
        {
            for (var i = challengesContainer.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(challengesContainer.transform.GetChild(i).gameObject);
            }
            challengesContainer.transform.DetachChildren();
        }
    }
}
