using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class MainMenuView : MonoBehaviour
    {
        [SerializeField] private GameObject _content;
        [SerializeField] private GameObject _backgroundPartsContainer;
        [SerializeField] private CanvasGroup _blackScreen;
        [Space]
        [SerializeField] private Button _levelsButton;
        [SerializeField] private Button _statisticsButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _exitButton;

        public GameObject Content => _content;
        public Button LevelsButton => _levelsButton;
        public Button StatisticsButton => _statisticsButton;
        public Button SettingsButton => _settingsButton;
        public Button ExitButton => _exitButton;

        public void SetContentLocalPosition(Vector3 localPosition) => _content.transform.localPosition = localPosition;

        public float GetContentHeight() => _content.GetComponent<RectTransform>().rect.height;

        public Image[] GetBackgroundParts() => _backgroundPartsContainer.GetComponentsInChildren<Image>();

        public void SetBlackScreenActive(bool isActive) => _blackScreen.gameObject.SetActive(isActive);

        public async UniTask SetBlackScreenAlphaAsync(float alpha, float duration)
        {
            await _blackScreen
                .DOFade(alpha, duration)
                .AsyncWaitForCompletion();
        }
    }
}
