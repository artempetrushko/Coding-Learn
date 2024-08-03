using Cysharp.Threading.Tasks;
using System.Linq;

namespace Scripts
{
    public class LevelsManager : IMainMenuSectionManager
    {
        private LevelsSectionController _levelsSectionController;
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

        public LevelsManager(LevelsSectionController levelsSectionController)
        {
            _levelsSectionController = levelsSectionController;
        }

        public async UniTask ShowSectionAsync()
        {
            _levelsSectionController.gameObject.SetActive(true);
            await _levelsSectionController.SetVisibilityAsync(true);
        }

        public async UniTask HideSectionAsync()
        {
            await _levelsSectionController.SetVisibilityAsync(false);
            _levelsSectionController.gameObject.SetActive(false);
        }

        public void CreateLevelButtons(int totalLevelsCount) => _levelsSectionController.CreateLevelButtons(totalLevelsCount);

        public void InitializeLevelsSection(LevelData[] levelDatas, int lastAvailableLevelNumber)
        {
            var availableLevelsDatas = levelDatas
                .Take(lastAvailableLevelNumber)
                .ToArray();
            _levelsSectionController.SetActiveLevelButtonsParams(availableLevelsDatas, SetLevelData);
            _levelsSectionController.MakeLevelButtonSelected(lastAvailableLevelNumber);
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
            _levelsSectionController.SetLevelInfo(levelData.Title.GetLocalizedString());
            //levelsSectionView.SetLevelThumbnail(levelData.LoadingScreen);
        }
    }
}
