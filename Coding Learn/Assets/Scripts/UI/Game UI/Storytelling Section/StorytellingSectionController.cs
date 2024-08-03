using Cysharp.Threading.Tasks;

namespace Scripts
{
    public class StorytellingSectionController
    {
        private StorytellingSectionView _storytellingSectionView;
        private bool isSkipButtonPressed = false;

        public StorytellingSectionController(StorytellingSectionView storytellingSectionView)
        {
            _storytellingSectionView = storytellingSectionView;
        }

        public async UniTask ShowStoryTextAsync(string storyText, float textShowingTime)
        {
            _storytellingSectionView.SetSkipStoryPartButtonActive(true);
            var latency = textShowingTime / storyText.Length;
            for (var i = 0; i < storyText.Length; i++)
            {
                if (isSkipButtonPressed)
                {
                    isSkipButtonPressed = false;
                    _storytellingSectionView.SetStoryText(storyText);
                    break;
                }
                _storytellingSectionView.AddStoryTextFragment(storyText[i].ToString());
                await UniTask.WaitForSeconds(latency);
            }
            _storytellingSectionView.SetSkipStoryPartButtonActive(false);
            _storytellingSectionView.SetNextStoryPartButtonActive(true);
        }

        public void SkipStoryTextShowing() => isSkipButtonPressed = true;

        public void ClearSection()
        {
            _storytellingSectionView.SetStoryText("");
            _storytellingSectionView.SetNextStoryPartButtonActive(false);
        }
    }
}
