using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic
{
    public class ErrorsSectionView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _errorsText;
        [SerializeField] private Scrollbar _scrollbar;

        public Vector3 LocalPosition => transform.localPosition;
        public Vector2 SizeDelta => GetComponent<RectTransform>().sizeDelta;

        public void SetActive(bool isActive) => gameObject.SetActive(isActive);

        public void SetErrorsText(string text) => _errorsText.text = text;

        public void SetScrollbarValue(float value) => _scrollbar.value = value;
    }
}
