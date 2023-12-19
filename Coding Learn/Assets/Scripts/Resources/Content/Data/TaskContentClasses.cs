using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Scripts
{
    public enum ChallengeType
    {
        SolveTask,
        NoTips,
        CompletingTimeLessThan
    }

    [Serializable]
    public class TaskInfo : Content
    {
        public string Title;
        public string Description;
        public string StartCode;
        public TestInfo TestInfo;
        public string[] Tips;
        public ChallengeInfo[] ChallengeInfos;
    }

    [Serializable]
    public class LocalizedTaskInfo : LocalizedContent
    {
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
}
