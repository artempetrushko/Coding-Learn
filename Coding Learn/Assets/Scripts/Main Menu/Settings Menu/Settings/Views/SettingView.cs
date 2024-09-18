using System;
using TMPro;
using UnityEngine;

namespace MainMenu
{
    public abstract class SettingView : MonoBehaviour
    {
        [SerializeField] protected TMP_Text _optionTitleText;
        [SerializeField] protected TMP_Text _optionValueText;

        public void SetOptionTitleText(string text) => _optionTitleText.text = text;

        public void SetOptionValueText(string text) => _optionValueText.text = text;
    }
}
