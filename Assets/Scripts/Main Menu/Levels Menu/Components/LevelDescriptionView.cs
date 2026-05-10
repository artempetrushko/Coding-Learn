using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class LevelDescriptionView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private Image[] _backgroundParts;

        public Image[] BackgroundParts => _backgroundParts;

        public void SetActive(bool isActive) => gameObject.SetActive(isActive);

        public void SetDescriptionText(string text) => _descriptionText.text = text;
    }
}
