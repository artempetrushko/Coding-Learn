namespace MainMenu
{
    public abstract class SwitchesSettingPresenter : SettingPresenter
    {
        protected readonly SwitchesSettingView _settingView;

        public SwitchesSettingPresenter(string saveKey, SwitchesSettingView view) : base(saveKey)
        {
            _settingView = view;

            _settingView.NextValueButton.onClick.AddListener(() => SetNeighbouringValue(1));
            _settingView.PreviousValueButton.onClick.AddListener(() => SetNeighbouringValue(-1));
        }

        protected abstract void SetNeighbouringValue(int orderNumberOffset);
    }
}
