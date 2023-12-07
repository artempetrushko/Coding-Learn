using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Scripts
{
    public class PadDevEnvironmentView : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField codeField;
        [SerializeField]
        private GameObject rowCounters;
        [SerializeField]
        private GameObject rowCounterPrefab;
        [SerializeField]
        private Button errorsButton;
        [SerializeField]
        private ErrorsSectionView errorsSectionView;
        [Space, SerializeField]
        private ProgrammingWordsHighlightData programmingWordsHighlightData;
        [Space, SerializeField]
        private PadDevEnvironmentAnimator animator;
    
        private GameObject rowCountersHolder;
        private TMP_TextInfo codeInfo;

        public string CodeFieldContent => codeField.text;

        public void SetDefaultCode(string defaultCode) => codeField.text = defaultCode;

        public void SetAndShowCompilationErrorsInfo(List<(int line, int column, string message)> errors)
        {
            var errorsMessage = string.Join("\n", errors.Select(error => string.Format("<color=red>Error</color> ({0}, {1}): {2}", error.line, error.column, error.message)));
            StartCoroutine(ShowErrors_COR(errorsMessage));
        }

        public IEnumerator ShowExecutingProcess_COR(bool isTaskCompleted)
        {
            yield return StartCoroutine(animator.ShowTaskSolutionChecking_COR(isTaskCompleted));
            if (isTaskCompleted)
            {

            }
            else
            {
                yield return StartCoroutine(ShowErrors_COR("Some of tests were failed! Try again!"));
            }
        }

        public void ChangeRowCountersScrollbarValue() => rowCounters.GetComponentInChildren<Scrollbar>().value = 1 - codeField.verticalScrollbar.value;

        public void LimitScrollbarValue() => codeField.verticalScrollbar.value = Mathf.Clamp01(codeField.verticalScrollbar.value);

        public void HighlightKeywords()
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

        public void ChangeRowCountersAmount()
        {
            var rowsCount = codeInfo.lineCount;
            var rowCountersAmount = rowCountersHolder.transform.childCount;
            if (rowCountersAmount != rowsCount)
            {
                if (rowCountersAmount < rowsCount)
                {
                    for (var i = rowCountersAmount + 1; i <= rowsCount; i++)
                    {
                        var rowCounter = Instantiate(rowCounterPrefab, rowCountersHolder.transform);
                        rowCounter.GetComponent<TMP_Text>().text = i.ToString();
                    }    
                }
                else
                {
                    for (var i = rowCountersAmount; i > rowsCount; i--)
                        Destroy(rowCountersHolder.transform.GetChild(i - 1).gameObject);
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

                Color32[] vertexColors = codeInfo.meshInfo[meshIndex].colors32;
                vertexColors[vertexIndex + 0] = selectedColor;
                vertexColors[vertexIndex + 1] = selectedColor;
                vertexColors[vertexIndex + 2] = selectedColor;
                vertexColors[vertexIndex + 3] = selectedColor;
            }
            codeField.textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
        }    

        private IEnumerator ShowErrors_COR(string errorsMessage)
        {
            yield return StartCoroutine(animator.ShowTaskCompletingIndicator_COR(false));
            errorsButton.interactable = true;
            errorsSectionView.SetContent(errorsMessage);
            StartCoroutine(errorsSectionView.ChangeVisibility_COR(true));         
        }

        private void Update()
        {
            //HighlightKeywords();
            //ChangeRowCountersAmount();
        }

        /*private void Start()
        {
            rowCountersHolder = rowCounters.transform.GetChild(0).GetChild(0).gameObject;
            codeInfo = codeField.textComponent.textInfo;
        }*/
    }
}
