using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic
{
    public class CodeFieldView : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _codeInputField;
        [SerializeField] private GameObject _rowCountersContainer;
        [SerializeField] private Scrollbar _rowCountersContainerScrollbar;

        public TMP_TextInfo CodeInputFieldTextInfo => _codeInputField.textComponent.textInfo;
        public GameObject RowCountersContainer => _rowCountersContainer;
        public string CodeInputFieldText
        {
            get => _codeInputField.text;
            set => _codeInputField.text = value;
        }

        public void UpdateCodeInputFieldVertexData() => _codeInputField.textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.All);

        public void SetRowCountersContainerScrollbarValue(float value) => _rowCountersContainerScrollbar.value = value;
    }
}
