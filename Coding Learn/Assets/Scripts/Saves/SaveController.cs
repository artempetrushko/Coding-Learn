using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Scripts
{
    public abstract class SaveController
    {
        public GameProgressData GameProgressData { get; protected set; }

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
    }
}
