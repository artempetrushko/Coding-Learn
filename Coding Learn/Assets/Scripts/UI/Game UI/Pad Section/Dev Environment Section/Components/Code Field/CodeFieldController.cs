using System.Linq;
using TMPro;
using UnityEngine;
using Zenject;

namespace Scripts
{
    public class CodeFieldController : ITickable
    {
        private CodeFieldView _codeFieldView;
        private RowCounterView _rowCounterPrefab;
        private ProgrammingWordsHighlightConfig _programmingWordsHighlightData;
        private DiContainer _diContainer;

        public CodeFieldController(DiContainer diContainer, CodeFieldView codeFieldView, RowCounterView rowCounterViewPrefab, ProgrammingWordsHighlightConfig programmingWordsHighlightData)
        {
            _diContainer = diContainer;
            _codeFieldView = codeFieldView;
            _rowCounterPrefab = rowCounterViewPrefab;
            _programmingWordsHighlightData = programmingWordsHighlightData;
        }

        public void Tick()
        {
            if (isPadVisible)
            {
                HighlightKeywords();
                UpdateRowCounters();
            }
        }

        public void SetDefaultCode(string code) => _codeFieldView.SetInputFieldText(code);

        private void HighlightKeywords()
        {
            var wordInfo = _codeFieldView.CodeInfo.wordInfo;
            for (var i = 0; i < _codeFieldView.CodeInfo.wordCount; i++)
            {
                var word = wordInfo[i].GetWord();
                var wordAndColorAccordance = _programmingWordsHighlightData.KeywordColors.Where(colorWordsPair => colorWordsPair.Keywords.Contains(word));
                if (wordAndColorAccordance.Count() != 0)
                {
                    PaintWordBySelectedColor(wordInfo[i], wordAndColorAccordance.First().Color);
                }
                else if (_codeFieldView.CodeInfo.characterInfo[wordInfo[i].lastCharacterIndex + 1].character == '(')
                {
                    PaintWordBySelectedColor(wordInfo[i], _programmingWordsHighlightData.MethodNameColor);
                }
            }
        }

        private void PaintWordBySelectedColor(TMP_WordInfo wordInfo, Color32 selectedColor)
        {
            for (var j = 0; j < wordInfo.characterCount; j++)
            {
                var charIndex = wordInfo.firstCharacterIndex + j;
                var meshIndex = _codeFieldView.CodeInfo.characterInfo[charIndex].materialReferenceIndex;
                var vertexIndex = _codeFieldView.CodeInfo.characterInfo[charIndex].vertexIndex;

                var vertexColors = _codeFieldView.CodeInfo.meshInfo[meshIndex].colors32;
                for (var i = 0; i <= 3; i++)
                {
                    vertexColors[vertexIndex + i] = selectedColor;
                }
            }
            _codeFieldView.UpdateAllVertexesData();
        }

        private void UpdateRowCounters()
        {
            for (var i = _codeFieldView.RowCountersContainer.transform.childCount - 1; i >= 0; i--)
            {
                Object.Destroy(_codeFieldView.RowCountersContainer.transform.GetChild(i).gameObject);
            }

            var lineInfos = _codeFieldView.CodeInfo.lineInfo.Where(lineInfo => lineInfo.characterCount > 0).ToArray();
            for (var i = 0; i < lineInfos.Length; i++)
            {
                var rowCounter = _diContainer.InstantiatePrefab(_rowCounterPrefab, _codeFieldView.RowCountersContainer.transform).GetComponent<RowCounterView>();
                rowCounter.SetCounterText((i + 1).ToString());
                rowCounter.SetCounterHeight(lineInfos[i].ascender - lineInfos[i].descender);

                if (i + 1 < lineInfos.Length)
                {
                    rowCounter.SetBottomGapActive(true);
                    rowCounter.SetBottomGapHeight(Mathf.Abs(lineInfos[i].descender - lineInfos[i + 1].ascender));
                }
            }
        }
    }
}
