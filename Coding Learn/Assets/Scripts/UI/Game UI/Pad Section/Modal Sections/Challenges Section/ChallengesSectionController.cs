using System.Collections.Generic;
using Zenject;

namespace Scripts
{
    public class ChallengesSectionController : PadModalWindow
    {
        private ChallengesSectionView _challengesScreenView;
        private PadChallengeView _challengeViewPrefab;
        private DiContainer _container;

        public ChallengesSectionController(DiContainer container, ChallengesSectionView challengesScreenView, PadChallengeView challengeViewPrefab)
        {
            _challengesScreenView = challengesScreenView;
            _challengeViewPrefab = challengeViewPrefab;
            _container = container;
        }

        public void CreateNewChallengeViews(List<string> challengeDescriptions)
        {
            DeletePreviousChallenges();
            foreach (string description in challengeDescriptions)
            {
                var challengeView = _container.InstantiatePrefab(_challengeViewPrefab, _challengesScreenView.ChallengesContainer.transform).GetComponent<PadChallengeView>();
                challengeView.SetDescriptionText(description);
            }
        }

        private void DeletePreviousChallenges()
        {
            for (var i = _challengesScreenView.ChallengesContainer.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(_challengesScreenView.ChallengesContainer.transform.GetChild(i).gameObject);
            }
            _challengesScreenView.ChallengesContainer.transform.DetachChildren();
        }
    }
}
