using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic
{
    public class HandbookThemeButton : MonoBehaviour
    {
        [SerializeField] private Button _buttonComponent;
        [SerializeField] private TMP_Text _themeLabel;

        public Button ButtonComponent => _buttonComponent;

        public void SetThemeText(string text) => _themeLabel.text = text;
    }
}
