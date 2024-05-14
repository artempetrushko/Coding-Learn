using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace Scripts
{
    public class LoadingBar : MonoBehaviour
    {
        [SerializeField]
        private Image innerArea;
        [SerializeField]
        private TMP_Text loadingText;

        public void SetContent(float loadingProgress)
        {
            innerArea.fillAmount = loadingProgress;
            loadingText.text = loadingText.GetComponent<LocalizeStringEvent>().StringReference.GetLocalizedString(Mathf.Round(loadingProgress * 100));
        }
    }
}
