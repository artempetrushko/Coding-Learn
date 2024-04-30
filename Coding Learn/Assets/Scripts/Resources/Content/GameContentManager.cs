using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Video;

namespace Scripts
{
    public class GameContentManager : ContentManager
    {
        private static LevelContent levelContent;

        public static (TimelineAsset, string[]) GetStoryPart(int questNumber)
        {
            var questStory = levelContent.Quests[questNumber - 1].Story;
            var storyParts = questStory.CutsceneScenarioParts.Select(part => part.GetLocalizedString()).ToArray();
            return (questStory.Cutscene, storyParts);
        }

        public static TaskContent GetTaskInfo(int taskNumber) => levelContent.Quests[taskNumber - 1].Task;

        public static TrainingTheme GetCodingTrainingTheme(int themeNumber) => null;//trainingThemes[themeNumber - 1];

        public static CodingTrainingData[] GetCodingTrainingInfos(int themeNumber, int subThemeNumber) => null;//trainingThemes[themeNumber - 1].SubThemes[subThemeNumber - 1].TrainingDatas;

        public static TrainingTheme[] GetHandbookTrainingThemes(int lastAvailableThemeNumber)
        {
            return null;/*trainingThemes
                .Take(lastAvailableThemeNumber)
                .Where(trainingTheme => FilterHandbookTrainingSubThemes(trainingTheme.SubThemes).Length > 0)
                .ToArray();*/
        }

        public static TrainingSubTheme[] GetHandbookTrainingSubThemes(TrainingTheme mainTheme, int? lastAvailableSubThemeNumber = null)
        {
            var subThemes = mainTheme.SubThemes;
            if (lastAvailableSubThemeNumber != null)
            {
                subThemes = subThemes.Take(lastAvailableSubThemeNumber.Value).ToArray();
            }
            return FilterHandbookTrainingSubThemes(subThemes);
        }

        public static CodingTrainingData[] GetHandbookCodingTrainingInfos(TrainingSubTheme subTheme)
        {
            return subTheme.TrainingDatas.Where(info => info.WillAddToHandbook).ToArray();
        }

        public static VideoClip GetTrainingVideo(string videoTitle)
        {
            /*foreach (var trainingTheme in trainingThemes)
            {
                foreach (var subTheme in trainingTheme.SubThemes)
                {
                    if (subTheme.TrainingDatas.Any(info => info.VideoTitle == videoTitle && info.IsTrainingContent))
                    {
                        return Resources.Load<VideoClip>(GeneralContentFolderPath + "/Game/Training Video/" + $"{trainingTheme.ID}/{subTheme.ID}/{videoTitle}");
                    }
                }
            }*/
            return null;
        }

        public static Sprite GetLoadingScreenBackground(int levelNumber) => Resources.Load<Sprite>($"Loading Screens/Loading Screen (Level {levelNumber})");

        private static TrainingSubTheme[] FilterHandbookTrainingSubThemes(TrainingSubTheme[] subThemes)
        {
            return subThemes.Where(subTheme => GetHandbookCodingTrainingInfos(subTheme).Length > 0).ToArray();
        }
    }
}
