using UnityEngine;

namespace Scripts
{
    public class SettingsSectionView : MonoBehaviour
    {
        [SerializeField] private GameObject _settingViewsContainer;

        public GameObject SettingViewsContainer => _settingViewsContainer;

        public float GetSectionHeight() => GetComponent<RectTransform>().rect.height;
    }
}
