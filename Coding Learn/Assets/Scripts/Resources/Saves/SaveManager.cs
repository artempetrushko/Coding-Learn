using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Scripts
{
    public abstract class SaveManager : MonoBehaviour
    {
        protected static GameProgressData gameProgressData;
        protected static SettingsData settingsData;

        private Dictionary<Type, string> savedDataFilePaths;

        public void Initialize()
        {
            savedDataFilePaths = new Dictionary<Type, string>()
            {
                { typeof(GameProgressData), Application.persistentDataPath + "/save.json" },
                { typeof(SettingsData),     Application.persistentDataPath + "/config.json" }
            };
        }

        protected void SerializeAndSaveData<T>(T data) where T : SavedData
        {
            var serializedData = JsonConvert.SerializeObject(data, Formatting.Indented);
            var dataFilePath = savedDataFilePaths[typeof(T)];
            File.WriteAllText(dataFilePath, serializedData);
            Debug.Log("Data was saved in " + dataFilePath);
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
                var serializedData = File.ReadAllText(dataFilePath);
                return JsonConvert.DeserializeObject<T>(serializedData);
            }
            catch
            {
                Debug.LogWarning($"File {dataFilePath} could not loaded!");
                return null;
            }
        }
    }
}
