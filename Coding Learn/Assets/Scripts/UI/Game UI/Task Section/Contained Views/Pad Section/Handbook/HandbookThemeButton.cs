using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scripts
{
    public class HandbookThemeButton : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text themeLabel;

        public void SetInfo(string theme, UnityAction buttonPressedAction)
        {
            themeLabel.text = theme;
            GetComponent<Button>().onClick.AddListener(buttonPressedAction);
        }
    }
}
