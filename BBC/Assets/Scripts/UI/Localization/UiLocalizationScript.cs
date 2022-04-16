using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Scripts
{
    [Serializable]
    public class GameUiLocalization
    {
        public string NextStoryPartButtonText;
        public string SkipStoryPartButtonText;
        public string RewardingPanelHeaderText;
        public string ExitToMenuPanelLabelText;
        public string ExitToMenuPanelYesButton;
        public string ExitToMenuPanelNoButton;
        public string LoadBarText;
    }

    public class UiLocalizationScript : MonoBehaviour
    {
        [SerializeField] private TMP_Text nextStoryPartButtonText;
        [SerializeField] private TMP_Text skipStoryPartButtonText;
        [SerializeField] private TMP_Text rewardingPanelHeaderText;
        [SerializeField] private Text exitToMenuPanelLabelText;
        [SerializeField] private Text exitToMenuPanelYesButton;
        [SerializeField] private Text exitToMenuPanelNoButton;
        [SerializeField] private Text loadBarText;

        private GameUiLocalization gameUiLocalization;

        public void GetResourcesByCurrentLanguage(Language language)
        {
            gameUiLocalization = JsonUtility.FromJson<GameUiLocalization>(Resources.Load<TextAsset>("Data/" + language.ToString() + "/Game UI/GameUI").text);
            FillUiTexts();
        }

        private void FillUiTexts()
        {
            nextStoryPartButtonText.text = gameUiLocalization.NextStoryPartButtonText;
            skipStoryPartButtonText.text = gameUiLocalization.SkipStoryPartButtonText;
            rewardingPanelHeaderText.text = gameUiLocalization.RewardingPanelHeaderText;
            exitToMenuPanelLabelText.text = gameUiLocalization.ExitToMenuPanelLabelText;
            exitToMenuPanelYesButton.text = gameUiLocalization.ExitToMenuPanelYesButton;
            exitToMenuPanelNoButton.text = gameUiLocalization.ExitToMenuPanelNoButton;
            loadBarText.text = gameUiLocalization.LoadBarText;
        }
    }
}
