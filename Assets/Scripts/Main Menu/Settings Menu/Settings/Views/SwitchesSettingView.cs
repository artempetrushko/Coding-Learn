using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class SwitchesSettingView : SettingView
    {
        [SerializeField] private Button _previousValueButton;
        [SerializeField] private Button _nextValueButton;

        public Button PreviousValueButton => _previousValueButton;
        public Button NextValueButton => _nextValueButton;
    }
}
