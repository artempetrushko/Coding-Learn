namespace Scripts
{
    public class TipsSectionController
    {
        private TipsSectionView _tipsScreenView;

        public TipsSectionController(TipsSectionView tipsScreenView)
        {
            _tipsScreenView = tipsScreenView;
        }

        public void AddNewTipText(string tip)
        {
            _tipsScreenView.SetTipFillerActive(false);
            _tipsScreenView.AddTipText(tip);
        }

        public void SetShowTipButtonInteractable(bool isInteractable) => _tipsScreenView.SetShowTipButtonInteractable(isInteractable);

        public void SetSkipTaskButtonInteractable(bool isInteractable) => _tipsScreenView.SetSkipTaskButtonInteractable(isInteractable);

        public void SetTipStatusText(string statusText) => _tipsScreenView.SetTipStatusText(statusText);

        public void SetSkipTaskButtonLabelText(string labelText) => _tipsScreenView.SetSkipTaskButtonLabelText(labelText);

        public void ClearTipText()
        {
            _tipsScreenView.SetTipText("");
            _tipsScreenView.SetTipFillerActive(true);
        }
    }
}