using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class SettingsMenuView : MonoBehaviour
    {
        [SerializeField] private GameObject _settingViewsContainer;
        [SerializeField] private Button _applySettingsButton;
        [SerializeField] private Button _backToMenuButton;

        public Button ApplySettingsButton => _applySettingsButton;
        public Button BackToMenuButton => _backToMenuButton;

        public GameObject SettingViewsContainer => _settingViewsContainer;

        public void SetActive(bool isActive) => gameObject.SetActive(isActive);

        public float GetSectionHeight() => GetComponent<RectTransform>().rect.height;
    }
}
