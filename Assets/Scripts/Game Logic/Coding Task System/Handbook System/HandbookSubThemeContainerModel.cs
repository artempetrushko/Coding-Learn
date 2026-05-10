namespace GameLogic
{
    public class HandbookSubThemeContainerModel
    {
        public readonly HandbookThemeButtonContainerView ContainerView;
        public readonly (HandbookThemeButton subThemeButton, TrainingSubTheme trainingSubTheme)[] SubThemeButtonDatas;

        public HandbookSubThemeContainerModel(HandbookThemeButtonContainerView containerView, (HandbookThemeButton subThemeButton, TrainingSubTheme trainingSubTheme)[] subThemeButtonDatas)
        {
            ContainerView = containerView;
            SubThemeButtonDatas = subThemeButtonDatas;
        }
    }
}
