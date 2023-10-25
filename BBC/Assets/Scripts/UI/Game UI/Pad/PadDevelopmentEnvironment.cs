using RoslynCSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using RoslynCSharp.Compiler;
using System.IO;

namespace Scripts
{
    public enum PadMode
    {
        Normal,
        Development,
        HandbookMainThemes,
        HandbookSubThemes,
        HandbookProgrammingInfo
    }

    public class PadDevelopmentEnvironment : MonoBehaviour
    {
        [HideInInspector] public string StartCode;

        #region UI-элементы
        [Header("Планшет")]
        public GameObject Pad;
        [Tooltip("Поле для ввода кода")]
        public TMP_InputField CodeField;
        [Tooltip("Номера строк кода")]
        public GameObject RowCounters;
        [Tooltip("Префаб счётчика строки")]
        public GameObject RowCounterPrefab;
        [Tooltip("Цветовой индикатор успешности выполнения задания")]
        public GameObject TaskStatusIndicator;
        [Header("Кнопки")]
        [Tooltip("Кнопка запуска программы")]
        public Button StartButton;
        [Tooltip("Кнопка показа ошибок")]
        public Button ErrorsButton;
        [Tooltip("Кнопка сброса кода к начальному состоянию")]
        public Button ResetButton;
        [Tooltip("Кнопка включения панели подсказок")]
        public Button TipButton;
        [Tooltip("Кнопка сворачивания планшета")]
        public Button RollPadButton;
        [Tooltip("Кнопка разворачивания планшета")]
        public Button UnrollPadButton;
        [Tooltip("Панель ошибок")]
        public GameObject ErrorsPanel;
        [Tooltip("Панель испытаний")]
        public GameObject ChallengesPanel;
        [Tooltip("Префаб испытания (UI)")]
        public GameObject ChallengePrefab;
        [Tooltip("Шкала прогресса выполнения программы")]
        public GameObject ExecutingProgressBar;
        #endregion

        #region Цвета для подсветки кода
        [Header("Подсветка кода")]
        [Tooltip("Подсветка ключевых слов")]
        [SerializeField] private Color32 keywordsColor;
        [Tooltip("Подсветка имён классов")]
        [SerializeField] private Color32 specifiersAndDataTypesColor;
        [Tooltip("Подсветка имён методов")]
        [SerializeField] private Color32 methodNameColor;
        [Tooltip("Подсветка имён локальных переменных")]
        [SerializeField] private Color32 localVariablesColor;
        #endregion

        #region Слова для подсветки
        [Header("Подсвечиваемые слова")]
        [SerializeField] private List<string> specifiers = new List<string>() { "public", "private", "static" };
        [SerializeField] private List<string> dataTypes = new List<string>() { "var", "int", "double", "float", "void", "bool", "true", "false", "char", "string", "long" };
        [SerializeField] private List<string> otherDarkBlueWords = new List<string>() { "new", "class", "enum" };
        [SerializeField] private List<string> keywords = new List<string>() { "if", "else", "for", "while", "switch", "case", "break", "continue", "yield", "try", "catch", "return" };
        #endregion

        [Space]
        [SerializeField] private UnityEvent onTaskCompleted;
    
        private GameManager gameManager;
        private GameObject rowCountersHolder;
        private TMP_TextInfo codeInfo;
        private Color32 successColor = Color.green;
        private Color32 errorColor = Color.red;
        private bool isErrorPanelShown = false;       

        public void RollPad() => StartCoroutine(RollPad_COR());

        public void UnrollPad() => StartCoroutine(UnrollPad_COR());

        public void OpenChallengesPanel() => StartCoroutine(PlayAnimation_COR(ChallengesPanel, "ScaleUp"));

        public void CloseChallengesPanel() => StartCoroutine(PlayAnimation_COR(ChallengesPanel, "ScaleDown"));

        public void ResetCode() => CodeField.text = StartCode;

        public void ToggleErrorPanelState()
        {
            ErrorsPanel.GetComponent<Animator>().Play(isErrorPanelShown ? "HideErrorPanel" : "ShowErrorPanel");
            isErrorPanelShown = !isErrorPanelShown;
        }

        public void ShowNewTaskCode()
        {
            var taskText = gameManager.GetCurrentTask();
            StartCode = taskText.StartCode;
            CodeField.text = taskText.StartCode;
            ErrorsButton.interactable = false;
            LoadNewChallenges();
            RollPad();
        }

        public void ExecuteCode()
        {
            if (isErrorPanelShown)
                ToggleErrorPanelState();
            ScriptDomain domain = ScriptDomain.CreateDomain("MyDomain");
            try
            {                             
                var robotManagementCode = gameManager.GetTests().Replace("//<playerCode>", CodeField.text);               
                ScriptType compiledCode = domain.CompileAndLoadMainSource(robotManagementCode);
                ScriptProxy proxy = compiledCode.CreateInstance(gameObject);
                StartCoroutine(ShowExecutingProcess(proxy));
            }
            catch
            {
                Debug.Log("Runtime code could not compile!");
                ErrorsButton.interactable = true;
                ExecutingProgressBar.SetActive(false);
                StartCoroutine(ShowErrors_COR(domain.GetErrors()));
            }
        }

        public void ChangeRowCountersScrollbarValue() => RowCounters.GetComponentInChildren<Scrollbar>().value = 1 - CodeField.verticalScrollbar.value;

        public void LimitScrollbarValue() => CodeField.verticalScrollbar.value = Mathf.Clamp01(CodeField.verticalScrollbar.value);

        public void HighlightKeywords()
        {
            var wordInfo = codeInfo.wordInfo;
            for (var i = 0; i < codeInfo.wordCount; i++)
            {
                var word = wordInfo[i].GetWord();
                if (specifiers.Contains(word) || dataTypes.Contains(word) || otherDarkBlueWords.Contains(word))
                    PaintWordBySelectedColor(wordInfo[i], specifiersAndDataTypesColor);
                else if (keywords.Contains(word))
                    PaintWordBySelectedColor(wordInfo[i], keywordsColor);
                else if (codeInfo.characterInfo[wordInfo[i].lastCharacterIndex + 1].character == '(')
                    PaintWordBySelectedColor(wordInfo[i], methodNameColor);
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
                        var rowCounter = Instantiate(RowCounterPrefab, rowCountersHolder.transform);
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
            CodeField.textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
        }

        private void LoadNewChallenges()
        {
            var challengesHolder = ChallengesPanel.GetComponentInChildren<VerticalLayoutGroup>().transform;
            for (var i = challengesHolder.transform.childCount; i > 0; i--)
                Destroy(challengesHolder.transform.GetChild(i - 1).gameObject);
            var challengeTexts = ResourcesData.TaskChallenges[gameManager.SceneIndex - 1][gameManager.GetCurrentTaskNumber() - 1];
            foreach (var challengeText in challengeTexts)
            {
                var challenge = Instantiate(ChallengePrefab, challengesHolder);
                challenge.GetComponentInChildren<TMP_Text>().text = challengeText.Challenge;
            }
        }

        private IEnumerator ShowErrors_COR(List<CompilationError> errors)
        {
            yield return StartCoroutine(TurnTaskIndicatorOn_COR(errorColor));
            var errorsTextField = ErrorsPanel.GetComponentInChildren<TMP_Text>();
            errorsTextField.text = "";         
            foreach (var error in errors)
            {
                errorsTextField.text += string.Format("<color=red>Error</color> ({0}, {1}): {2}\n", error.SourceLine - 5, error.SourceColumn, error.Message);
            }              
            if (!isErrorPanelShown)
                ToggleErrorPanelState();
        }

        private IEnumerator ShowExecutingProcess(ScriptProxy proxy)
        {
            var isTaskCompleted = (bool)proxy.Call("isTaskCompleted");
            ExecutingProgressBar.SetActive(true);
            ExecutingProgressBar.GetComponent<Animator>().SetBool("isTaskCompleted", true);
            yield return StartCoroutine(PlayAnimation_COR(ExecutingProgressBar, "FillProgressBar"));
            if (isTaskCompleted)
            {               
                gameManager.IsTimerStopped = true;
                yield return StartCoroutine(TurnTaskIndicatorOn_COR(successColor));
                onTaskCompleted.Invoke();
                ExecutingProgressBar.GetComponent<Animator>().SetBool("isTaskCompleted", false);
            }
            else
            {
                ExecutingProgressBar.SetActive(false);
                ErrorsButton.interactable = true;
                yield return StartCoroutine(TurnTaskIndicatorOn_COR(errorColor));
                ErrorsPanel.GetComponentInChildren<TMP_Text>().text = "Some of tests were failed! Try again!";
                if (!isErrorPanelShown)
                    ToggleErrorPanelState();
            }
        }

        private IEnumerator TurnTaskIndicatorOn_COR(Color indicatorColor)
        {
            TaskStatusIndicator.SetActive(true);
            TaskStatusIndicator.GetComponent<Image>().color = indicatorColor;
            yield return StartCoroutine(PlayAnimation_COR(TaskStatusIndicator, "TurnIndicatorOn"));
            TaskStatusIndicator.SetActive(false);
        }

        private IEnumerator RollPad_COR()
        {
            if (isErrorPanelShown)
                ToggleErrorPanelState();
            RollPadButton.interactable = false;
            yield return StartCoroutine(PlayAnimation_COR(gameObject, "RollPad"));
            UnrollPadButton.gameObject.SetActive(true);
        }

        private IEnumerator UnrollPad_COR()
        {
            UnrollPadButton.gameObject.SetActive(false);
            yield return StartCoroutine(PlayAnimation_COR(gameObject, "UnrollPad"));
            RollPadButton.interactable = true;
        }

        private IEnumerator PlayAnimation_COR(GameObject animatorHolder, string animationName)
        {
            var animator = animatorHolder.GetComponent<Animator>();
            var clip = animator.runtimeAnimatorController.animationClips.Where(x => x.name == animationName).First();
            animator.Play(clip.name);
            yield return new WaitForSeconds(clip.length);
        }

        private void LaunchCompiler()
        {
            ScriptDomain domain = ScriptDomain.CreateDomain("MyDomain");
            ScriptType type = domain.CompileAndLoadMainSource(@"
using UnityEngine;
using System;

public class LaunchClass : MonoBehaviour
{
    public void LaunchCompiler() => Debug.Log(""Compiler is working!"");
}");
            ScriptProxy proxy = type.CreateInstance(gameObject);
            proxy.Call("LaunchCompiler");
        }

        private void Update()
        {
            HighlightKeywords();
            ChangeRowCountersAmount();
        }

        private void Start()
        {
            gameManager = GameManager.Instance;
            rowCountersHolder = RowCounters.transform.GetChild(0).GetChild(0).gameObject;
            codeInfo = CodeField.textComponent.textInfo;
            LaunchCompiler();
        }
    }
}
