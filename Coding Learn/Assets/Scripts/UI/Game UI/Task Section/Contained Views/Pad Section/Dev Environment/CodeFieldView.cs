using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class CodeFieldView : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField codeField;
        [Space, SerializeField]
        private RowCounterView rowCounterPrefab;
        [SerializeField]
        private GameObject rowCountersContainer;
        [SerializeField]
        private Scrollbar rowCountersContainerScrollbar;
        [Space, SerializeField]
        private ProgrammingWordsHighlightData programmingWordsHighlightData;

        private bool isPadVisible;
        private TMP_TextInfo codeInfo;

        public string CodeFieldContent => codeField.text;

        public void SetPadVisibilityState(bool isVisible) => isPadVisible = isVisible;

        public void SetDefaultCode(string defaultCode) => codeField.text = defaultCode;

        public void ChangeRowCountersScrollbarValue() => rowCountersContainerScrollbar.value = 1 - codeField.verticalScrollbar.value;

        public void LimitScrollbarValue() => codeField.verticalScrollbar.value = Mathf.Clamp01(codeField.verticalScrollbar.value);

        private void OnEnable()
        {
            codeInfo = codeField.textComponent.textInfo;
        }

        private void Update()
        {
            if (isPadVisible)
            {
                HighlightKeywords();
                UpdateRowCounters();
            }
        }

        private void HighlightKeywords()
        {
            var wordInfo = codeInfo.wordInfo;
            for (var i = 0; i < codeInfo.wordCount; i++)
            {
                var word = wordInfo[i].GetWord();
                var wordAndColorAccordance = programmingWordsHighlightData.KeywordColors.Where(colorWordsPair => colorWordsPair.Keywords.Contains(word));
                if (wordAndColorAccordance.Count() != 0)
                {
                    PaintWordBySelectedColor(wordInfo[i], wordAndColorAccordance.First().Color);
                }
                else if (codeInfo.characterInfo[wordInfo[i].lastCharacterIndex + 1].character == '(')
                {
                    PaintWordBySelectedColor(wordInfo[i], programmingWordsHighlightData.MethodNameColor);
                }
            }
        }

        private void PaintWordBySelectedColor(TMP_WordInfo wordInfo, Color32 selectedColor)
        {
            for (var j = 0; j < wordInfo.characterCount; j++)
            {
                var charIndex = wordInfo.firstCharacterIndex + j;
                var meshIndex = codeInfo.characterInfo[charIndex].materialReferenceIndex;
                var vertexIndex = codeInfo.characterInfo[charIndex].vertexIndex;

                var vertexColors = codeInfo.meshInfo[meshIndex].colors32;
                for (var i = 0; i <= 3; i++)
                {
                    vertexColors[vertexIndex + i] = selectedColor;
                }
            }
            codeField.textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
        }

        private void UpdateRowCounters()
        {
            for (var i = rowCountersContainer.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(rowCountersContainer.transform.GetChild(i).gameObject);
            }

            var lineInfos = codeInfo.lineInfo.Where(lineInfo => lineInfo.characterCount > 0).ToArray();
            for (var i = 0; i < lineInfos.Length; i++)
            {
                var rowCounter = Instantiate(rowCounterPrefab, rowCountersContainer.transform);
                rowCounter.SetParams((i + 1).ToString(), lineInfos[i].ascender - lineInfos[i].descender);
                if (i + 1 < lineInfos.Length)
                {
                    rowCounter.AddBottomGap(Mathf.Abs(lineInfos[i].descender - lineInfos[i + 1].ascender));
                }
            }
        }
    }
}
