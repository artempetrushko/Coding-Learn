using System;
using UnityEngine;
using UnityEngine.Localization;

namespace Scripts
{
    [Serializable]
    public class SettingData
    {
        [SerializeField] private GameSettingType _type;
        [SerializeField] private LocalizedString _name;
        [SerializeField] private SettingViewType _viewType;

        public GameSettingType Type => _type;
        public LocalizedString Name => _name;
        public SettingViewType ViewType => _viewType;
    }
}
