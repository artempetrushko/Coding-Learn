namespace GameLogic
{
    public class HandbookModel
    {
        public readonly (HandbookThemeButton mainThemeButton, HandbookSubThemeContainerModel subThemesContainerModel)[] MainThemeButtonDatas;

        public HandbookSubThemeContainerModel SelectedSubThemesContainer;

        public HandbookModel((HandbookThemeButton mainThemeButton, HandbookSubThemeContainerModel subThemesContainerModel)[] mainThemeButtonDatas)
        {
            MainThemeButtonDatas = mainThemeButtonDatas;
        }
    }
}
