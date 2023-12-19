using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts
{
    public class MainMenuContentManager : ContentManager
    {
        private static TaskInfo[][] availableLevelsTaskInfos;
        private static LevelInfo[] levelInfos;
        private static Sprite[] loadingScreens;

        public static TaskInfo GetTaskInfo(int levelNumber, int taskNumber) => availableLevelsTaskInfos[levelNumber - 1][taskNumber -  1];

        public static TaskInfo[] GetLevelTaskInfos(int levelNumber) => availableLevelsTaskInfos[levelNumber - 1];

        public static LevelInfo GetLevelInfo(int levelNumber) => levelInfos[levelNumber - 1];

        public static LevelInfo[] GetAvailableLevelInfos(int availableLevelsCount) => levelInfos.Take(availableLevelsCount).ToArray();

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
            levelInfos = LoadDatasFromFile<LevelInfo>(LocalizedContentFolderPath + "/Main Menu/Level Infos").Take(availableLevelsCount).ToArray();

            availableLevelsTaskInfos = new TaskInfo[availableLevelsCount][];
            for (var i = 0; i < availableLevelsTaskInfos.Length; i++)
            {
                availableLevelsTaskInfos[i] = LoadTaskInfos(i + 1);
            }
        }
    }
}
