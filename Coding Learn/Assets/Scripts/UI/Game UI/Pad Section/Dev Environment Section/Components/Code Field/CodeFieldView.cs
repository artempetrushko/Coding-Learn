using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class CodeFieldView : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _codeInputField;
        [SerializeField] private GameObject _rowCountersContainer;
        [SerializeField] private Scrollbar _rowCountersContainerScrollbar;

        public TMP_TextInfo CodeInfo => _codeInputField.textComponent.textInfo;
        public string CodeFieldContent => _codeInputField.text;
        public GameObject RowCountersContainer => _rowCountersContainer;

        public void SetInputFieldText(string text) => _codeInputField.text = text;

        public void ChangeRowCountersScrollbarValue() => _rowCountersContainerScrollbar.value = 1 - _codeInputField.verticalScrollbar.value;

        public void LimitScrollbarValue() => _codeInputField.verticalScrollbar.value = Mathf.Clamp01(_codeInputField.verticalScrollbar.value);

        public void UpdateAllVertexesData() => _codeInputField.textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
    }
}
