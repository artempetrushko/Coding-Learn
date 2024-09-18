using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class SettingsMenuView : MonoBehaviour
    {
        [SerializeField] private GameObject _settingViewsContainer;
        [SerializeField] private Button _applySettingsButton;
        [SerializeField] private Button _closeViewButton;

        public Button ApplySettingsButton => _applySettingsButton;
        public Button CloseViewButton => _closeViewButton;

        public GameObject SettingViewsContainer => _settingViewsContainer;

        public void SetActive(bool isActive) => gameObject.SetActive(isActive);

        public float GetSectionHeight() => GetComponent<RectTransform>().rect.height;
    }
}
