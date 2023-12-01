using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Scripts
{
    public abstract class ContentManager : MonoBehaviour
    {
        protected string contentRootFolderPath = "Data/";

        protected string LocalizedContentFolderPath => contentRootFolderPath + LocalizationSettings.SelectedLocale.Identifier.Code;

        protected T[] LoadDatasFromFile<T>(string resourcePath)
        {
            var resources = Resources.Load<TextAsset>(resourcePath);
            return DeserializeData<T[]>(resources);
        }

        protected T[][] LoadDatasFromFiles<T>(string commonFilePath, int filesCount)
        {
            var datas = new T[filesCount][];
            for (var i = 1; i <= filesCount; i++)
            {
                var file = Resources.Load<TextAsset>(commonFilePath + i);
                datas[i] = DeserializeData<T[]>(file);
            }
            return datas;
        }

        protected T DeserializeData<T>(TextAsset serializedData)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(serializedData.text);
            }
            catch
            {
                Debug.LogError("Некорректный текст JSON в файле " + serializedData.name);
                return default;
            }
        }
    }
}
