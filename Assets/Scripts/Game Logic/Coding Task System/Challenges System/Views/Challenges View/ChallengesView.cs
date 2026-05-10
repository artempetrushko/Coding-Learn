using UnityEngine;
using UnityEngine.UI;

namespace GameLogic
{
    public class ChallengesView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [Space]
        [SerializeField] private GameObject _challengesContainer;       
        [SerializeField] private Button _closeViewButton;

        public GameObject ChallengesContainer => _challengesContainer;
        public CanvasGroup CanvasGroup => _canvasGroup;
        public Button CloseViewButton => _closeViewButton;

        public void SetActive(bool isActive) => gameObject.SetActive(isActive);
    }
}
