using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;

namespace Scripts
{
    public class GameContentManager : ContentManager
    {
        private static StoryInfo[] storyParts;
        private static TrainingTheme[] codingTrainingInfos;
        private static TaskInfo[] taskInfos;
        private static string[] tests;     

        public static StoryInfo GetStoryInfo(int cutsceneNumber) => storyParts[cutsceneNumber - 1];

        public static TrainingTheme GetCodingTrainingTheme(int themeNumber) => codingTrainingInfos[themeNumber - 1];

        public static TrainingTheme[] GetFirstCodingTrainingThemes(int themesCount) => codingTrainingInfos.Take(themesCount).ToArray();

        public static TaskInfo GetTaskInfo(int taskNumber) => taskInfos[taskNumber - 1];

        public static string GetTests(int taskNumber) => tests[taskNumber - 1];

        public static VideoClip GetTrainingVideo(string videoTitle) => Resources.Load<VideoClip>("Video/" + videoTitle);

        public void LoadContentFromResources(int currentLevelNumber)
        {
            storyParts = LoadDatasFromFile<StoryInfo>(LocalizedContentFolderPath + "/Game/Story/Story Level " + currentLevelNumber);
            taskInfos = LoadDatasFromFile<TaskInfo>(LocalizedContentFolderPath + "/Game/Tasks/Tasks Level " + currentLevelNumber);
            tests = Resources.LoadAll<TextAsset>("Tests/Level " + currentLevelNumber).Select(file => file.text).ToArray();

            codingTrainingInfos = new TrainingTheme[currentLevelNumber];
            for (var i = 1; i <= codingTrainingInfos.Length; i++)
            {
                var file = Resources.Load<TextAsset>(LocalizedContentFolderPath + "/Game/Coding Training Level" + i);
                codingTrainingInfos[i - 1] = DeserializeData<TrainingTheme>(file);
            }
        }
    }
}
