namespace MainMenu
{
    public abstract class SliderSettingPresenter : SettingPresenter
    {
        protected readonly SliderSettingView _settingView;
        protected readonly int _minSettingValue;
        protected readonly int _maxSettingValue;

        protected int _currentSettingValue;

        public SliderSettingPresenter(string saveKey, SliderSettingView view, int minSettingValue, int maxSettingValue) : base(saveKey)
        {
            _settingView = view;
            _minSettingValue = minSettingValue;
            _maxSettingValue = maxSettingValue;

            _currentSettingValue = ES3.Load(_saveKey, _maxSettingValue);

            _settingView.Slider.minValue = _minSettingValue;
            _settingView.Slider.maxValue = _maxSettingValue;
            _settingView.Slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        private void OnSliderValueChanged(float value)
        {
            _currentSettingValue = (int)value;
            _settingView.SetOptionValueText(_currentSettingValue.ToString());
        }
    }
}
