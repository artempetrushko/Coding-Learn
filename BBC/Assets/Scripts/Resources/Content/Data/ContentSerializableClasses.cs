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
    public class TaskInfo
    {
        public string Title;
        public string Description;
        public string StartCode;
        public string[] Tips;
        public ChallengeInfo[] ChallengeInfos;
    }

    [Serializable]
    public class StoryInfo
    {
        public string[] Articles;
    }

    [Serializable]
    public class TrainingTheme
    {
        public string Title;
        public TrainingSubTheme[] SubThemes;
    }

    [Serializable]
    public class TrainingSubTheme
    {
        public string Title;
        public CodingTrainingInfo[] Infos;
    }

    [Serializable]
    public class CodingTrainingInfo
    {
        public string Title;
        public string Info;
        public string VideoTitle;
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
    public class LevelInfo
    {
        public string LevelTitle;
        public string Description;
    }
}
