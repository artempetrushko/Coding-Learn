using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LevelLoading
{
    public class LevelLoadingView : MonoBehaviour
    {
        [SerializeField] private Image _background;
        [Space]
        [SerializeField] private GameObject _loadingBar;
        [SerializeField] private Image _loadingBarInnerArea;
        [SerializeField] private TMP_Text _loadingBarText;

        public Image Background => _background;

        public void SetActive(bool isActive) => gameObject.SetActive(isActive);

        public void SetLoadingBarActive(bool isActive) => _loadingBar.SetActive(isActive);

        public void SetBackgroundSprite(Sprite sprite) => _background.sprite = sprite;

        public void SetLoadingBarFillAmount(float fillAmount) => _loadingBarInnerArea.fillAmount = fillAmount;

        public void SetLoadingBarText(string text) => _loadingBarText.text = text;
    }
}
