using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Scripts
{
    public abstract class SaveManager : MonoBehaviour
    {
        protected static SaveData saveData;

        protected string SaveFilePath => Application.persistentDataPath + "/save.json";

        protected void SerializeAndSaveData(SaveData data)
        {
            var serializedData = JsonConvert.SerializeObject(data);
            File.WriteAllText(SaveFilePath, serializedData);
            Debug.Log("������ ��������� � " + SaveFilePath);
        }

        protected SaveData LoadSavedData()
        {
            try
            {
                var serializedData = File.ReadAllText(SaveFilePath);
                return JsonConvert.DeserializeObject<SaveData>(serializedData);
            }
            catch
            {
                Debug.Log(string.Format("���� {0} �� ��� ��������!", SaveFilePath));
                return null;
            }
        }
    }
}
