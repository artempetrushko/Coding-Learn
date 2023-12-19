using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Video;

namespace Scripts
{
    public class GameContentManager : ContentManager
    {
        private static StoryInfo[] storyInfos;
        private static TrainingMainTheme[] trainingThemes;
        private static TaskInfo[] taskInfos;

        public static (TimelineAsset, string[]) GetStoryPart(int levelNumber, int storyPartNumber)
        {
            var storyInfo = storyInfos[storyPartNumber - 1];
            var cutscene = Resources.Load<TimelineAsset>($"Timelines/Cutscenes/Level {levelNumber}/{storyInfo.CutsceneTitle}");
            return (cutscene, storyInfo.Articles);
        }

        public static TaskInfo GetTaskInfo(int taskNumber) => taskInfos[taskNumber - 1];

        public static TrainingMainTheme GetCodingTrainingTheme(int themeNumber) => trainingThemes[themeNumber - 1];

        public static CodingTrainingInfo[] GetCodingTrainingInfos(int themeNumber, int subThemeNumber) => trainingThemes[themeNumber - 1].SubThemes[subThemeNumber - 1].Infos;

        public static TrainingMainTheme[] GetHandbookTrainingThemes(int lastAvailableThemeNumber)
        {
            return trainingThemes
                .Take(lastAvailableThemeNumber)
                .Where(trainingTheme => FilterHandbookTrainingSubThemes(trainingTheme.SubThemes).Length > 0)
                .ToArray();
        }

        public static TrainingSubTheme[] GetHandbookTrainingSubThemes(TrainingMainTheme mainTheme, int? lastAvailableSubThemeNumber = null)
        {
            var subThemes = mainTheme.SubThemes;
            if (lastAvailableSubThemeNumber != null)
            {
                subThemes = subThemes.Take(lastAvailableSubThemeNumber.Value).ToArray();
            }
            return FilterHandbookTrainingSubThemes(subThemes);
        }

        public static CodingTrainingInfo[] GetHandbookCodingTrainingInfos(TrainingSubTheme subTheme)
        {
            return subTheme.Infos.Where(info => info.IsTrainingContent).ToArray();
        }

        public static VideoClip GetTrainingVideo(string videoTitle)
        {
            foreach (var trainingTheme in trainingThemes)
            {
                foreach (var subTheme in trainingTheme.SubThemes)
                {
                    if (subTheme.Infos.Any(info => info.VideoTitle == videoTitle && info.IsTrainingContent))
                    {
                        return Resources.Load<VideoClip>(GeneralContentFolderPath + "/Game/Training Video/" + $"{trainingTheme.ID}/{subTheme.ID}/{videoTitle}");
                    }
                }
            }
            return null;
        }

        public static Sprite GetLoadingScreenBackground(int levelNumber) => Resources.Load<Sprite>($"Loading Screens/Loading Screen (Level {levelNumber})");

        public void LoadContentFromResources(int levelNumber)
        {
            storyInfos = LoadContentWithLocalizedData<StoryInfo, LocalizedStoryInfo>(levelNumber, ("/Game/Story/", "Story Level "), ("/Game/Story/", "Story Level "));
            taskInfos = LoadTaskInfos(levelNumber);
            trainingThemes = LoadTrainingThemes(levelNumber);
        }

        private static TrainingSubTheme[] FilterHandbookTrainingSubThemes(TrainingSubTheme[] subThemes)
        {
            return subThemes.Where(subTheme => GetHandbookCodingTrainingInfos(subTheme).Length > 0).ToArray();
        }

        private TrainingMainTheme[] LoadTrainingThemes(int levelNumber)
        {
            trainingThemes = new TrainingMainTheme[levelNumber];
            for (var i = 1; i <= trainingThemes.Length; i++)
            {
                var trainingThemeData = DeserializeData<TrainingMainTheme>(Resources.Load<TextAsset>(GeneralContentFolderPath + "/Game/Coding Training/Coding Training Level " + i));
                var localizedTrainingThemeData = DeserializeData<LocalizedTrainingTheme>(Resources.Load<TextAsset>(LocalizedContentFolderPath + "/Game/Localized Coding Training/Localized Coding Training Level " + i));

                trainingThemeData.Title = localizedTrainingThemeData.Title;
                foreach (var subTheme in trainingThemeData.SubThemes)
                {
                    var accordingLocalizedSubThemeData = localizedTrainingThemeData.SubThemes.Where(localizedSubTheme => localizedSubTheme.LinkedContentID == subTheme.ID).FirstOrDefault();
                    if (accordingLocalizedSubThemeData != null)
                    {
                        subTheme.Title = accordingLocalizedSubThemeData.Title;
                        PopulateGeneralContentWithLocalizedData(subTheme.Infos, accordingLocalizedSubThemeData.Infos);
                    }
                }
                trainingThemes[i - 1] = trainingThemeData;
            }
            return trainingThemes;
        }
    }
}
