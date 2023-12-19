using System;
using System.Collections;
using System.Collections.Generic;

namespace Scripts
{
    public class TrainingTheme : Content
    {
        public string Title;
    }

    [Serializable]
    public class TrainingMainTheme : TrainingTheme
    {
        public TrainingSubTheme[] SubThemes;
    }

    [Serializable]
    public class LocalizedTrainingTheme : LocalizedContent
    {
        public string Title;
        public LocalizedTrainingSubTheme[] SubThemes;
    }

    [Serializable]
    public class TrainingSubTheme : TrainingTheme
    {
        public CodingTrainingInfo[] Infos;
    }

    [Serializable]
    public class LocalizedTrainingSubTheme : LocalizedContent
    {
        public string Title;
        public LocalizedCodingTrainingInfo[] Infos;
    }

    [Serializable]
    public class CodingTrainingInfo : Content
    {
        public bool IsTrainingContent;
        public string Title;
        public string Info;
        public string VideoTitle;
    }

    [Serializable]
    public class LocalizedCodingTrainingInfo : LocalizedContent
    {
        public string Title;
        public string Info;
    }
}
