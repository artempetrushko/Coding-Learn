using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Scripts
{
    public class SaveManager
    {
        public static GameProgressData GameProgressData { get; protected set; }
        public static SettingsData SettingsData { get; private set; }

        private Dictionary<Type, string> savedDataFilePaths = new()
        {
            { typeof(GameProgressData), Application.persistentDataPath + "/save.json" },
            { typeof(SettingsData),     Application.persistentDataPath + "/config.json" }
        };

        protected void SerializeAndSaveData<T>(T data) where T : SavedData
        {
            var serializedData = JsonConvert.SerializeObject(data, Formatting.Indented);
            var dataFilePath = savedDataFilePaths[typeof(T)];
            using (var writer = new StreamWriter(dataFilePath))
            {
                writer.Write(serializedData);
                Debug.Log("Data was saved in " + dataFilePath);
            }
        }

        protected T LoadOrCreateSavedData<T>(T defaultSavedData) where T : SavedData
        {
            var savedData = LoadSavedData<T>();
            if (savedData != null)
            {
                return savedData;
            }
            SerializeAndSaveData(defaultSavedData);
            return defaultSavedData;
        }

        protected T LoadSavedData<T>() where T: SavedData
        {
            var dataFilePath = savedDataFilePaths[typeof(T)];
            try
            {
                using (var reader = new StreamReader(dataFilePath))
                {
                    var serializedData = reader.ReadToEnd();
                    return JsonConvert.DeserializeObject<T>(serializedData);
                }                
            }
            catch
            {
                Debug.LogWarning($"File {dataFilePath} can't be loaded!");
                return null;
            }
        }



        public TaskChallengesResults GetCurrentTaskChallengesResults(int currentTaskNumber)
        {
            var tasksChallengesResults = GameProgressData.AllChallengeStatuses[0].TasksChallengesResults; //0 is temporary
            if (currentTaskNumber > tasksChallengesResults.Count)
            {
                tasksChallengesResults.Add(new TaskChallengesResults());
            }
            return tasksChallengesResults[currentTaskNumber - 1];
        }

        public void SaveProgress(int lastAvailableNumber)
        {
            if (lastAvailableNumber > GameProgressData.LastAvailableLevelNumber)
            {
                GameProgressData.LastAvailableLevelNumber = lastAvailableNumber;
            }
            SerializeAndSaveData(GameProgressData);
        }

        public void LoadSaveData() => GameProgressData = LoadSavedData<GameProgressData>();





        

        public void LoadOrCreateAllSavedData(int levelsCount)
        {
            GameProgressData = LoadOrCreateSavedData(new GameProgressData()
            {
                LastAvailableLevelNumber = 1,
                AllChallengeStatuses = new LevelChallengesResults[levelsCount].Select(item => item = new LevelChallengesResults()).ToArray()
            });
            SettingsData = LoadOrCreateSavedData(new SettingsData()
            {
                Resolution = string.Format(@"{0} x {1}", Screen.currentResolution.width, Screen.currentResolution.height),
                FullScreenMode = Screen.fullScreenMode,
                GraphicsQuality = QualitySettings.names[QualitySettings.GetQualityLevel()],
                Language = LocalizationSettings.SelectedLocale.LocaleName.Split()[0],
                SoundsVolume = 100,
                MusicVolume = 100,
            });
        }

        public void SaveSettings() => SerializeAndSaveData(SettingsData);
    }
}
