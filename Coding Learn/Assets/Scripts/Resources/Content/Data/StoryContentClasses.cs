using System;

namespace Scripts
{
    [Serializable]
    public class StoryInfo : Content
    {
        public string CutsceneTitle;
        public string[] Articles;
    }

    [Serializable]
    public class LocalizedStoryInfo : LocalizedContent
    {
        public string[] Articles;
    }
}
