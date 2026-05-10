using GameLogic;
using UI.MainMenu;

namespace MainMenu
{
    public class LevelsMenuModel
    {
        public readonly LevelConfig[] LevelConfigs;

        public (LevelButton levelButton, LevelConfig linkedLevelConfig)[] LevelButtonConfigs;
        public LevelConfig SelectedLevelConfig;

        public LevelsMenuModel(LevelConfig[] levelConfigs)
        {
            LevelConfigs = levelConfigs;
        }
    }
}
