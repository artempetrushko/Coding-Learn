using UI.MainMenu;

namespace MainMenu
{
    public class StatisticsMenuModel
    {
        public readonly (LevelStatisticsCardView levelStatisticsCard, TaskStatisticsPageView linkedStatisticsPage)[] LevelStatisticsCardDatas;

        public TaskStatisticsPageView SelectedStatisticsPage;

        public StatisticsMenuModel((LevelStatisticsCardView levelStatisticsCard, TaskStatisticsPageView linkedStatisticsPage)[] levelStatisticsCardDatas)
        {
            LevelStatisticsCardDatas = levelStatisticsCardDatas;
        }
    }
}
