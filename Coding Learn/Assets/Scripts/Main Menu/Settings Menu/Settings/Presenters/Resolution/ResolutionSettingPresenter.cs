using System.Linq;
using UnityEngine;

namespace MainMenu
{
    public class ResolutionSettingPresenter : SwitchesSettingPresenter
    {
        private (Resolution resolution, string formattedResolution)[] _settingValues;
        private int _currentValueOrderNumber;

        public ResolutionSettingPresenter(string saveKey, SwitchesSettingView view) : base(saveKey, view)
        {
            _settingValues = Screen.resolutions
                .Select(resolution => (resolution, $"{resolution.width} x {resolution.height}"))
                .ToArray();
        }

        public override void ApplyValue()
        {
            var selectedResolution = _settingValues[_currentValueOrderNumber - 1].resolution;
            Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);

            ES3.Save(_saveKey, selectedResolution);
        }

        protected override void SetNeighbouringValue(int orderNumberOffset)
        {
            throw new System.NotImplementedException();
        }
    }
}
