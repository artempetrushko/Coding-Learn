using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    public class ChallengesRewardingView : MonoBehaviour
    {
        [SerializeField] private Button _closeViewButton;
        [SerializeField] private GameObject _challengeViewsContainer;

        public GameObject ChallengeViewsContainer => _challengeViewsContainer;
        public Button CloseViewButton => _closeViewButton;

        public void SetActive(bool isActive) => gameObject.SetActive(isActive);
    }
}
