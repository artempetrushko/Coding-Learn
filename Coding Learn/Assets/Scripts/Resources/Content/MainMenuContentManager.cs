using System.Linq;
using UnityEngine;

namespace Scripts
{
    public class MainMenuContentManager : ContentManager
    {
        private static TaskContent[][] availableLevelsTaskInfos;
        private static MainMenuLevelInfo[] levelInfos;
        private static Sprite[] loadingScreens;

        public static TaskContent GetTaskInfo(int levelNumber, int taskNumber) => availableLevelsTaskInfos[levelNumber - 1][taskNumber -  1];

        public static TaskContent[] GetLevelTaskInfos(int levelNumber) => availableLevelsTaskInfos[levelNumber - 1];

        public static MainMenuLevelInfo GetLevelInfo(int levelNumber) => levelInfos[levelNumber - 1];

        public static MainMenuLevelInfo[] GetAvailableLevelInfos(int availableLevelsCount) => levelInfos.Take(availableLevelsCount).ToArray();

        public static Sprite GetLoadingScreen(int levelNumber) => loadingScreens[levelNumber - 1];

        public void LoadContentFromResources(int availableLevelsCount)
        {
            LoadTextContent(availableLevelsCount);

            loadingScreens = new Sprite[availableLevelsCount];
            for (var i = 1; i <= availableLevelsCount; i++)
            {
                loadingScreens[i - 1] = Resources.Load<Sprite>($"Loading Screens/Loading Screen (Level {i})");
            }       
        }

        public void LoadTextContent(int availableLevelsCount)
        {
            /*levelInfos = LoadDatasFromFile<LevelInfo>(LocalizedContentFolderPath + "/Main Menu/Level Infos").Take(availableLevelsCount).ToArray();

            availableLevelsTaskInfos = new TaskContent[availableLevelsCount][];
            for (var i = 0; i < availableLevelsTaskInfos.Length; i++)
            {
                availableLevelsTaskInfos[i] = LoadTaskInfos(i + 1);
            }*/
        }
    }
}
