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

        public static StoryInfo GetStoryInfo(int cutsceneNumber) => storyParts[cutsceneNumber - 1];

        public static TrainingTheme GetCodingTrainingTheme(int themeNumber) => codingTrainingInfos[themeNumber - 1];

        public static TrainingTheme[] GetFirstCodingTrainingThemes(int themesCount) => codingTrainingInfos.Take(themesCount).ToArray();

        public static TaskInfo GetTaskInfo(int taskNumber) => taskInfos[taskNumber - 1];

        public static VideoClip GetTrainingVideo(int themeNumber, int subThemeNumber, string videoTitle)
        {
            var currentTrainingTheme = GetCodingTrainingTheme(themeNumber);
            var currentTrainingSubTheme = currentTrainingTheme.SubThemes[subThemeNumber - 1];
            return Resources.Load<VideoClip>(GeneralContentFolderPath + "/Game/Training Video/" + $"{currentTrainingTheme.ThemeID}/{currentTrainingSubTheme.SubThemeID}/{videoTitle}");
        }

        public static Sprite GetLoadingScreenBackground(int levelNumber) => Resources.Load<Sprite>(string.Format(@"Loading Screens/Loading Screen (Level {0})", levelNumber));

        public void LoadContentFromResources(int currentLevelNumber)
        {
            storyParts = LoadDatasFromFile<StoryInfo>(LocalizedContentFolderPath + "/Game/Story/Story Level " + currentLevelNumber);
            taskInfos = LoadTaskInfos(currentLevelNumber);

            codingTrainingInfos = LoadCodingTrainingInfos(currentLevelNumber);
            /*codingTrainingInfos = new TrainingTheme[currentLevelNumber];
            for (var i = 1; i <= codingTrainingInfos.Length; i++)
            {
                var file = Resources.Load<TextAsset>(LocalizedContentFolderPath + "/Game/Coding Training/Coding Training Level " + i);
                codingTrainingInfos[i - 1] = DeserializeData<TrainingTheme>(file);
            }*/
        }

        private TaskInfo[] LoadTaskInfos(int currentLevelNumber)
        {
            var generalTaskInfos = LoadDatasFromFile<TaskInfo>(GeneralContentFolderPath + "/Game/Tasks/Tasks Level " + currentLevelNumber);
            var localizedTaskInfos = LoadDatasFromFile<LocalizedTaskInfo>(LocalizedContentFolderPath + "/Game/Localized Task Datas/Localized Task Datas Level " + currentLevelNumber);
            foreach (var generalTaskInfo in generalTaskInfos)
            {
                var accordingLocalizedTaskInfo = localizedTaskInfos.Where(localizedTaskInfo => localizedTaskInfo.LinkedTaskID == generalTaskInfo.ID).First();
                generalTaskInfo.Title = accordingLocalizedTaskInfo.Title;
                generalTaskInfo.Description = accordingLocalizedTaskInfo.Description;
                generalTaskInfo.Tips = accordingLocalizedTaskInfo.Tips;
            }
            return generalTaskInfos;
        }

        private TrainingTheme[] LoadCodingTrainingInfos(int currentLevelNumber)
        {
            codingTrainingInfos = new TrainingTheme[currentLevelNumber];
            for (var i = 1; i <= codingTrainingInfos.Length; i++)
            {
                var trainingThemeData = DeserializeData<TrainingTheme>(Resources.Load<TextAsset>(GeneralContentFolderPath + "/Game/Coding Training/Coding Training Level " + i));
                var localizedTrainingThemeData = DeserializeData<LocalizedTrainingTheme>(Resources.Load<TextAsset>(LocalizedContentFolderPath + "/Game/Localized Coding Training/Localized Coding Training Level " + i));
                trainingThemeData.Title = localizedTrainingThemeData.Title;
                foreach (var subTheme in trainingThemeData.SubThemes)
                {
                    var accordingLocalizedSubThemeData = localizedTrainingThemeData.SubThemes
                        .Where(localizedSubTheme => localizedSubTheme.LinkedSubThemeID == subTheme.SubThemeID)
                        .FirstOrDefault();
                    if (accordingLocalizedSubThemeData != null)
                    {
                        subTheme.Title = accordingLocalizedSubThemeData.Title;
                        foreach (var codingTrainingInfo in subTheme.Infos)
                        {
                            var accordingLocalizedTrainingInfoData = accordingLocalizedSubThemeData.Infos
                                .Where(localizedTrainingInfo => localizedTrainingInfo.LinkedTrainingInfoID == codingTrainingInfo.TrainingInfoID)
                                .FirstOrDefault();
                            if (accordingLocalizedTrainingInfoData != null)
                            {
                                codingTrainingInfo.Title = accordingLocalizedTrainingInfoData.Title;
                                codingTrainingInfo.Info = accordingLocalizedTrainingInfoData.Info;
                            }
                        }
                    }
                }
                codingTrainingInfos[i - 1] = trainingThemeData;
            }
            return codingTrainingInfos;
        }
    }
}
