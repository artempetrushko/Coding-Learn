using Leguar.TotalJSON;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Scripts
{
    public class MainMenuSaveManager : MonoBehaviour
    {
        public static SaveData SaveData { get; private set; }

        private string SaveFilePath => Application.persistentDataPath + "/save.json";

        public void LoadOrCreateSaveData()
        {
            var savedData = LoadSavedData();
            SaveData = savedData != null ? savedData : CreateDefaultSaveFile();
        }

        public void ChangeLanguageData(string newLanguageCode)
        {
            SaveData.LanguageCode = newLanguageCode;
            SerializeAndWriteData(SaveData);
        }

        private void SerializeAndWriteData(SaveData savedData)
        {
            var serializedData = JSON.Serialize(savedData).CreatePrettyString();
            File.WriteAllText(SaveFilePath, serializedData);
            Debug.Log("Данные сохранены в " + SaveFilePath);
        }

        private SaveData CreateDefaultSaveFile()
        {
            var savedData = new SaveData();
            savedData.LanguageCode = "ru";
            savedData.LastAvailableLevelNumber = 1;
            savedData.ChallengeCompletingStatuses = new List<List<List<bool>>> { new List<List<bool>> { new List<bool> { } } };
            return savedData;
        }

        private SaveData LoadSavedData()
        {
            try
            {
                var serializedData = File.ReadAllText(SaveFilePath);
                return JSON.ParseString(serializedData).Deserialize<SaveData>();
            }
            catch
            {
                return null;
            }
        }
    }
}
