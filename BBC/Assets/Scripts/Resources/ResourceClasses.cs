using System;
using UnityEngine;

namespace Scripts
{ 
    public class Letter
    {
        public string Title;
        public string Description;
    }

    [Serializable]
    public class TaskText : Letter
    {
        public int ID;
        public string StartCode;
    }

    [Serializable]
    public class Story
    {
        public string Text;
    }

    [Serializable]
    public class TipMessage
    {
        public string Tip;
    }

    [Serializable]
    public class ThemeTitle
    {
        public string Title;
    }

    [Serializable]
    public class CodingTrainingInfo
    {
        public string Title;
        public string Info;
        public string VideoTitles;
    }

    [Serializable]
    public class Challenges
    {
        public string Challenge;
        public double CheckValue;
    }

    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array, bool prettyPrint = false)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }
}
