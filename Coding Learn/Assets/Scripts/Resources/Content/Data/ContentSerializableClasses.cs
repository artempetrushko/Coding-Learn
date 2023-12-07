using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Scripts
{ 
    public enum ChallengeType
    {
        SolveTask,
        NoTips,
        CompletingTimeLessThan
    }

    [Serializable]
    public class StoryInfo
    {
        [JsonProperty]
        private string[] Articles;

        public string GetArticle(int articleNumber) => Articles[articleNumber - 1];
    }

    [Serializable]
    public class TrainingTheme
    {
        public string ThemeID;
        public string Title;
        public TrainingSubTheme[] SubThemes;
    }

    [Serializable]
    public class LocalizedTrainingTheme
    {
        public string LinkedThemeID;
        public string Title;
        public LocalizedTrainingSubTheme[] SubThemes;
    }

    [Serializable]
    public class TrainingSubTheme
    {
        public string SubThemeID;
        public string Title;
        public CodingTrainingInfo[] Infos;
    }

    [Serializable]
    public class LocalizedTrainingSubTheme
    {
        public string LinkedSubThemeID;
        public string Title;
        public LocalizedCodingTrainingInfo[] Infos;
    }

    [Serializable]
    public class CodingTrainingInfo
    {
        public string TrainingInfoID;
        public bool IsTrainingContent;
        public string Title;
        public string Info;
        public string VideoTitle;
    }

    [Serializable]
    public class LocalizedCodingTrainingInfo
    {
        public string LinkedTrainingInfoID;
        public string Title;
        public string Info;
    }

    [Serializable]
    public class TaskInfo
    {
        public string ID;
        public string Title;
        public string Description;
        public string StartCode;
        public TestInfo TestInfo;
        public string[] Tips;
        public ChallengeInfo[] ChallengeInfos;
    }

    [Serializable]
    public class LocalizedTaskInfo
    {
        public string LinkedTaskID;
        public string Title;
        public string Description;
        public string[] Tips;
    }

    [Serializable]
    public class TestInfo
    {
        public string TestCode;
        public string TestMethodName;
        public string PlayerCodePlaceholder;
        public int PlayerCodeStartRowNumber;
    }

    [Serializable]
    public class ChallengeInfo
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ChallengeType Type;
        public string Description;
        public object CheckValue;
    }

    [Serializable]
    public class LocalizedChallengeInfo
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public ChallengeType Type;
        public string Description;
    }

    [Serializable]
    public class LevelInfo
    {
        public string LevelTitle;
        public string Description;
    }
}
