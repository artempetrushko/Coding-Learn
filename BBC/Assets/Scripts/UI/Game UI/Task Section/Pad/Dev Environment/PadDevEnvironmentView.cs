using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RoslynCSharp.Compiler;

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
        private GameObject taskStatusIndicator;
        [SerializeField]
        private Button errorsButton;
        [SerializeField]
        private ErrorsSectionView errorsSectionView;
        [SerializeField]
        private GameObject executingProgressBar;
        [Space, SerializeField]
        private ProgrammingWordsHighlightData programmingWordsHighlightData;
    
        private GameObject rowCountersHolder;
        private TMP_TextInfo codeInfo;
        private Color successColor = Color.green;
        private Color errorColor = Color.red;
        private bool isErrorPanelShown = false;

        public string CodeFieldContent => codeField.text;

        public void SetDefaultCode(string defaultCode) => codeField.text = defaultCode;

        public void SetAndShowCompilationErrorsInfo(List<CompilationError> errors)
        {
            var errorsMessage = string.Join("\n", errors.Select(error => string.Format("<color=red>Error</color> ({0}, {1}): {2}", error.SourceLine - 5, error.SourceColumn, error.Message)));
            StartCoroutine(ShowErrors_COR(errorsMessage));
        }

        public IEnumerator ShowExecutingProcess_COR(bool isTaskCompleted)
        {
            executingProgressBar.SetActive(true);
            executingProgressBar.GetComponent<Animator>().SetBool("isTaskCompleted", true);
            yield return StartCoroutine(PlayAnimation_COR(executingProgressBar, "FillProgressBar"));
            if (isTaskCompleted)
            {
                yield return StartCoroutine(TurnTaskIndicatorOn_COR(successColor));
                executingProgressBar.GetComponent<Animator>().SetBool("isTaskCompleted", false);
            }
            else
            {
                yield return StartCoroutine(ShowErrors_COR("Some of tests were failed! Try again!"));
            }
        }

        public void ToggleErrorPanelState()
        {
            errorsSectionView.GetComponent<Animator>().Play(isErrorPanelShown ? "HideErrorPanel" : "ShowErrorPanel");
            isErrorPanelShown = !isErrorPanelShown;
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
            yield return StartCoroutine(TurnTaskIndicatorOn_COR(errorColor));
            errorsButton.interactable = true;
            errorsSectionView.SetContent(errorsMessage);
            if (!isErrorPanelShown)
            {
                ToggleErrorPanelState();
            }            
        }

        private IEnumerator TurnTaskIndicatorOn_COR(Color indicatorColor)
        {
            taskStatusIndicator.SetActive(true);
            taskStatusIndicator.GetComponent<Image>().color = indicatorColor;
            yield return StartCoroutine(PlayAnimation_COR(taskStatusIndicator, "TurnIndicatorOn"));
            taskStatusIndicator.SetActive(false);
        }

        private IEnumerator PlayAnimation_COR(GameObject animatorHolder, string animationName)
        {
            var animator = animatorHolder.GetComponent<Animator>();
            var clip = animator.runtimeAnimatorController.animationClips.Where(x => x.name == animationName).First();
            animator.Play(clip.name);
            yield return new WaitForSeconds(clip.length);
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
