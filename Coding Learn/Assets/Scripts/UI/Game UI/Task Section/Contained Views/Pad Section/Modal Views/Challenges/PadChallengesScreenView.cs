using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class PadChallengesScreenView : PadModalWindow
    {
        [Space, SerializeField]
        private PadChallengeView challengeViewPrefab;
        [SerializeField]
        private GameObject challengesContainer;

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
