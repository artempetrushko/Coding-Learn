using Cysharp.Threading.Tasks;
using System.Linq;

namespace Scripts
{
    public class LevelsManager : IMainMenuSectionManager
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

        public LevelsManager(LevelsSectionView levelsSectionView)
        {
            this.levelsSectionView = levelsSectionView;
        }

        public async UniTask ShowSectionAsync()
        {
            levelsSectionView.gameObject.SetActive(true);
            await levelsSectionView.ChangeVisibilityAsync(true);
        }

        public async UniTask HideSectionAsync()
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
