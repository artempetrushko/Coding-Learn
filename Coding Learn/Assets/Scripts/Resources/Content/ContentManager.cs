using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Scripts
{
    public abstract class ContentManager : MonoBehaviour
    {
        protected static string contentRootFolderPath = "Content";

        protected static string GeneralContentFolderPath => contentRootFolderPath + "/General";
        protected static string LocalizedContentFolderPath => contentRootFolderPath + "/Localized Content/" + LocalizationSettings.SelectedLocale.Identifier.Code;

        protected TaskInfo[] LoadTaskInfos(int levelNumber)
        {
            var taskInfos = LoadContentWithLocalizedData<TaskInfo, LocalizedTaskInfo>(levelNumber, ("/Game/Tasks/", "Tasks Level "), ("/Game/Localized Task Datas/", "Localized Task Datas Level "));
            var localizedChallengesInfos = LoadDatasFromFile<LocalizedChallengeInfo>(LocalizedContentFolderPath + "/Game/Challenge Descriptions/Challenge Descriptions");
            foreach (var taskInfo in taskInfos)
            {
                foreach (var challengeInfo in taskInfo.ChallengeInfos)
                {
                    var accordingLocalizedChallengeData = localizedChallengesInfos.Where(localizedChallengesInfo => localizedChallengesInfo.Type == challengeInfo.Type).FirstOrDefault();
                    if (accordingLocalizedChallengeData != null)
                    {
                        challengeInfo.Description = accordingLocalizedChallengeData.Description;
                    }
                }
            }
            return taskInfos;
        }

        protected T[] LoadContentWithLocalizedData<T, P>(int levelNumber, (string relativeFolderPath, string fileNameMask) generalContentFileData, (string relativeFolderPath, string fileNameMask) localizedContentFileData)
            where T : Content
            where P : LocalizedContent
        {
            var generalContent = LoadDatasFromFile<T>(GeneralContentFolderPath + generalContentFileData.relativeFolderPath + generalContentFileData.fileNameMask + levelNumber);
            var localizedContent = LoadDatasFromFile<P>(LocalizedContentFolderPath + localizedContentFileData.relativeFolderPath + localizedContentFileData.fileNameMask + levelNumber);
            PopulateGeneralContentWithLocalizedData(generalContent, localizedContent);
            return generalContent;
        }

        protected void PopulateGeneralContentWithLocalizedData<T, P>(T[] generalContent, P[] localizedContent)
            where T : Content
            where P : LocalizedContent
        {
            foreach (var generalContentPart in generalContent)
            {
                var accordingLocalizedData = localizedContent.Where(localizedData => localizedData.LinkedContentID == generalContentPart.ID).FirstOrDefault();
                if (accordingLocalizedData != null)
                {
                    JsonConvert.PopulateObject(JsonConvert.SerializeObject(accordingLocalizedData), generalContentPart);
                }
            }
        }

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
