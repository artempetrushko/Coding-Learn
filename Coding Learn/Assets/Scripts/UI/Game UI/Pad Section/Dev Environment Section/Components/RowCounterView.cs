using TMPro;
using UnityEngine;

namespace Scripts
{
    public class RowCounterView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _counterText;
        [SerializeField] private RectTransform _bottomGap;

        public void SetCounterText(string text) => _counterText.text = text;

        public void SetCounterHeight(float height) => _counterText.rectTransform.sizeDelta = new Vector2(_counterText.rectTransform.rect.width, height);

        public void SetBottomGapActive(bool isActive) => _bottomGap.gameObject.SetActive(isActive);

        public void SetBottomGapHeight(float size) => _bottomGap.sizeDelta = new Vector2(_bottomGap.rect.width, size);
    }
}
