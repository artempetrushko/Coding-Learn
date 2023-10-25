using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public enum Language
    {
        RU, 
        EN
    }

    [Serializable]
    public class LevelInfo
    {
        public string LevelTitle;
        public string Description;
    }

    [Serializable]
    public class MenuUiLocalization
    {
        public string PlayButton_MainMenuText;
        public string StatsButtonText;
        public string SettingsButtonText;
        public string ExitButtonText;
        public string PlayButton_LevelPanelText;
        public string PlayButton_LevelPanel_SavedLevelText;
        public string ResolutionOptionText;
        public string ScreenModeOptionText;
        public string QualityLevelOptionText;
        public string LanguageOptionText;
        public string SoundSliderText;
        public string MusicSliderText;
        public string BackToMenuButtonText;
        public string ApplyButtonText;
        public string LoadBarText;
        public string LevelStatsPanelTitleText;
    }

    public class MenuLocalizationScript : MonoBehaviour
    {
        #region UI-элементы для локализации
        [SerializeField] private Text playButton_MainMenuText;
        [SerializeField] private Text statsButtonText;
        [SerializeField] private Text settingsButtonText;
        [SerializeField] private Text exitButtonText;
        [SerializeField] private Text playButton_LevelPanelText;
        [SerializeField] private Text resolutionOptionText;
        [SerializeField] private Text screenModeOptiontext;
        [SerializeField] private Text qualityLevelOptionText;
        [SerializeField] private Text languageOptionText;
        [SerializeField] private Text soundSliderText;
        [SerializeField] private Text musicSliderText;
        [SerializeField] private Text backToMenuButtonText;
        [SerializeField] private Text applyButtonText;
        [SerializeField] private Text loadBarText;
        [SerializeField] private Text levelStatsPanelTitleText;
        #endregion

        [Space]
        [SerializeField] private Text languageSubOptionText;

        private Language currentLanguage;
        private LevelInfo[] levelInfos;
        private MenuUiLocalization menuUiLocalization;

        public LevelInfo GetLevelInfo(int levelNumber) => levelInfos[levelNumber - 1];

        public string GetPlayButtonText() => menuUiLocalization.PlayButton_LevelPanelText;

        public string GetPlayButtonText_SavedLevel() => menuUiLocalization.PlayButton_LevelPanel_SavedLevelText;

        public string GetLoadBarText() => menuUiLocalization.LoadBarText;

        public void ChooseNextLanguage() => ChangeLanguage(1);

        public void ChoosePreviousLanguage() => ChangeLanguage(-1);

        private void ChangeLanguage(int enumOffset)
        {
            var newLanguageEnumValue = (int)currentLanguage + enumOffset;
            var languageEnumValuesCount = Enum.GetNames(typeof(Language)).Length;
            if (newLanguageEnumValue < 0)
                currentLanguage = (Language)languageEnumValuesCount - 1;
            else if (newLanguageEnumValue >= languageEnumValuesCount)
                currentLanguage = 0;
            else currentLanguage = (Language)newLanguageEnumValue;
            switch (currentLanguage)
            {
                case Language.RU:
                    languageSubOptionText.text = "Русский";
                    break;
                case Language.EN:
                    languageSubOptionText.text = "English";
                    break;
            }
            GetResourcesByCurrentLanguage();
            SaveManager.ChangeLanguageData(currentLanguage);
        }

        public void GetResourcesByCurrentLanguage()
        {
            levelInfos = GetResourcesAndWrite<LevelInfo>("Data/" + currentLanguage.ToString() + "/Menu/LevelInfos");
            menuUiLocalization = JsonUtility.FromJson<MenuUiLocalization>(Resources.Load<TextAsset>("Data/" + currentLanguage.ToString() + "/Menu/MenuUI").text);
            FillUiTexts();
        }

        private void FillUiTexts()
        {
            /*var menuUiLocalizationFields = typeof(MenuUiLocalization).GetFields();
            var menuLocalizationScriptFields = typeof(MenuLocalizationScript).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in menuUiLocalizationFields)
            {
                var fieldValue = field.GetValue(menuUiLocalization);
                var currentClassField = menuLocalizationScriptFields.Where(x => string.Equals(x.Name, field.Name, StringComparison.OrdinalIgnoreCase)).First();
                var textProperty = currentClassField.FieldType.GetProperty("text");
                textProperty.SetValue(currentClassField.FieldType, fieldValue);
            }*/
            playButton_MainMenuText.text = menuUiLocalization.PlayButton_MainMenuText;
            statsButtonText.text = menuUiLocalization.StatsButtonText;
            settingsButtonText.text = menuUiLocalization.SettingsButtonText;
            exitButtonText.text = menuUiLocalization.ExitButtonText;
            playButton_LevelPanelText.text = menuUiLocalization.PlayButton_LevelPanelText;
            resolutionOptionText.text = menuUiLocalization.ResolutionOptionText;
            screenModeOptiontext.text = menuUiLocalization.ScreenModeOptionText;
            qualityLevelOptionText.text = menuUiLocalization.QualityLevelOptionText;
            languageOptionText.text = menuUiLocalization.LanguageOptionText;
            soundSliderText.text = menuUiLocalization.SoundSliderText;
            musicSliderText.text = menuUiLocalization.MusicSliderText;
            backToMenuButtonText.text = menuUiLocalization.BackToMenuButtonText;
            applyButtonText.text = menuUiLocalization.ApplyButtonText;
            loadBarText.text = menuUiLocalization.LoadBarText;
            levelStatsPanelTitleText.text = menuUiLocalization.LevelStatsPanelTitleText;
        }

        private T[] GetResourcesAndWrite<T>(string resourcePath)
        {
            var resources = Resources.Load<TextAsset>(resourcePath);
            return JsonHelper.FromJson<T>(resources.text);
        }

        private void OnEnable()
        {
            currentLanguage = SaveManager.SaveData.Language;
            //ChangeLanguage(0);
        }
    }
}
