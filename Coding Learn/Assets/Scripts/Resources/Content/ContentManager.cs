using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Scripts
{
    public abstract class ContentManager : MonoBehaviour
    {
        protected static string contentRootFolderPath = "Content";

        protected static string GeneralContentFolderPath => contentRootFolderPath + "/General";
        protected static string LocalizedContentFolderPath => contentRootFolderPath + "/Localized Content/" + LocalizationSettings.SelectedLocale.Identifier.Code;

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
                datas[i - 1] = DeserializeData<T[]>(file);
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
                Debug.LogError($"File {serializedData.name} contains incorrect JSON text");
                return default;
            }
        }
    }
}
