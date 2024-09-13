namespace MainMenu
{
    public abstract class SettingPresenter
    {
        protected readonly string _saveKey;

        public SettingPresenter(string saveKey)
        {
            _saveKey = saveKey;
        }

        public abstract void ApplyValue();
    }
}
