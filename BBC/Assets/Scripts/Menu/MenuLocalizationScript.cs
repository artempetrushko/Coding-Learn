using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts
{
    enum Language
    {
        RU, 
        EN
    }

    [Serializable]
    public class LevelInfo
    {
        public string LevelTitle;
    }

    public class MenuLocalizationScript : MonoBehaviour
    {
        private Language currentLanguage;
        private LevelInfo[] levelInfos;

        public LevelInfo GetLevelInfo(int levelNumber) => levelInfos[levelNumber - 1];

        private void GetResourcesByCurrentLanguage()
        {
            levelInfos = GetResourcesAndWrite<LevelInfo>("Localization/LevelTitles/" + currentLanguage.ToString() + "/LevelTitles");
        }

        private T[] GetResourcesAndWrite<T>(string resourcePath)
        {
            var resources = Resources.Load<TextAsset>(resourcePath);
            return GameManager.JsonHelper.FromJson<T>(resources.text);
        }

        private void Awake()
        {
            currentLanguage = Language.RU;  
            GetResourcesByCurrentLanguage();
        }
    }
}
