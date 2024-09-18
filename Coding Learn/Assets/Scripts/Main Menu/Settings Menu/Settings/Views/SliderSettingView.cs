using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class SliderSettingView : SettingView
    {
        [SerializeField] private Slider _slider;

        public Slider Slider => _slider;
    }
}
