using Cysharp.Threading.Tasks;
using System.Linq;

namespace Scripts
{
    public class LevelsController : IMainMenuSectionController
    {
        private LevelsSectionView levelsSectionView;
        private LevelData selectedLevelData;

        private LevelData SelectedLevelData
        {
            get => selectedLevelData;
            set
            {
                if (selectedLevelData != value)
                {
                    selectedLevelData = value;
                    if (selectedLevelData != null)
                    {
                        ShowLevelData(selectedLevelData);
                    }                 
                }
            }
        }

        public LevelsController(LevelsSectionView levelsSectionView)
        {
            this.levelsSectionView = levelsSectionView;
        }

        public async UniTask ShowSectionViewAsync()
        {
            levelsSectionView.gameObject.SetActive(true);
            await levelsSectionView.ChangeVisibilityAsync(true);
        }

        public async UniTask HideSectionViewAsync()
        {
            await levelsSectionView.ChangeVisibilityAsync(false);
            levelsSectionView.gameObject.SetActive(false);
        }

        public void CreateLevelButtons(int totalLevelsCount) => levelsSectionView.CreateLevelButtons(totalLevelsCount);

        public void InitializeLevelsSection(LevelData[] levelDatas, int lastAvailableLevelNumber)
        {
            var availableLevelsDatas = levelDatas
                .Take(lastAvailableLevelNumber)
                .ToArray();
            levelsSectionView.SetActiveLevelButtonsParams(availableLevelsDatas, SetLevelData);
            levelsSectionView.MakeLevelButtonSelected(lastAvailableLevelNumber);
            if (SelectedLevelData != levelDatas[lastAvailableLevelNumber - 1])
            {
                SelectedLevelData = levelDatas[lastAvailableLevelNumber - 1];
            }
            else
            {
                ShowLevelData(levelDatas[lastAvailableLevelNumber - 1]);
            }
        }

        private void SetLevelData(LevelData levelData) => SelectedLevelData = levelData;

        private void ShowLevelData(LevelData levelData)
        {
            levelsSectionView.SetLevelInfo(levelData.Title.GetLocalizedString());
            //levelsSectionView.SetLevelThumbnail(levelData.LoadingScreen);
        }
    }
}
