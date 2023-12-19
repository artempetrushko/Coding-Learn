using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Scripts
{
    public class RowCounterView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text counterText;
        [SerializeField]
        private RectTransform bottomGap;

        public void SetParams(string text, float height)
        {
            counterText.text = text;
            counterText.rectTransform.sizeDelta = new Vector2(counterText.rectTransform.rect.width, height);
        }

        public void AddBottomGap(float size)
        {
            bottomGap.gameObject.SetActive(true);
            bottomGap.sizeDelta = new Vector2(bottomGap.rect.width, size);
        }
    }
}
