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

        public void SetInfo(string themeTitle, UnityAction buttonPressedAction)
        {
            themeLabel.text = themeTitle;
            GetComponent<Button>().onClick.AddListener(buttonPressedAction);
        }
    }
}
