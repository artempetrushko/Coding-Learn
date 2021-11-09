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

namespace Scripts
{
    public class PadDevelopmentBehaviour : MonoBehaviour
    {
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

        [Header("Подсветка кода")]
        [Tooltip("Подсветка ключевых слов")]
        [SerializeField] private Color32 keywordsColor;
        [Tooltip("Подсветка имён классов")]
        [SerializeField] private Color32 classNameColor;
        [Tooltip("Подсветка имён методов")]
        [SerializeField] private Color32 methodNameColor;
        [Tooltip("Подсветка имён локальных переменных")]
        [SerializeField] private Color32 localVariablesColor;

        [Space]
        [SerializeField] private UnityEvent onTaskCompleted;

        [HideInInspector] public string StartCode;

        private GameManager gameManager;
        private GameObject rowCountersHolder;
        private Color successColor = new Color(0f, 1f, 0f, 0f);
        private Color errorColor = new Color(1f, 0f, 0f, 0f);
        private List<string> keywords = new List<string>() { "public", "private", "static", "var", "int", "double", "float", "void" };
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
            var taskText = gameManager.TaskTexts[gameManager.CurrentTaskNumber - 1];
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
                var robotManagementCode = GetRobotManagementClass(gameManager.GetTests().ExtraCode);               
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

        public void HighlightKeywords()
        {
            var textComponent = CodeField.textComponent;
            var wordInfo = textComponent.textInfo.wordInfo;
            for (var i = 0; i < textComponent.textInfo.wordCount; i++)
            {
                if (keywords.Contains(wordInfo[i].GetWord()))
                {
                    for (var j = 0; j < wordInfo[i].characterCount; j++)
                    {
                        var charIndex = wordInfo[i].firstCharacterIndex + j;
                        var meshIndex = textComponent.textInfo.characterInfo[charIndex].materialReferenceIndex;
                        var vertexIndex = textComponent.textInfo.characterInfo[charIndex].vertexIndex;

                         /*CodeField.textComponent.textInfo.characterInfo[charIndex].vertex_BL.color = Color.blue;
                         CodeField.textComponent.textInfo.characterInfo[charIndex].vertex_BR.color = Color.blue;
                         CodeField.textComponent.textInfo.characterInfo[charIndex].vertex_TL.color = Color.blue;
                         CodeField.textComponent.textInfo.characterInfo[charIndex].vertex_TR.color = Color.blue;*/

                        Color32[] vertexColors = textComponent.textInfo.meshInfo[meshIndex].colors32;
                        vertexColors[vertexIndex + 0] = Color.blue;
                        vertexColors[vertexIndex + 1] = Color.blue;
                        vertexColors[vertexIndex + 2] = Color.blue;
                        vertexColors[vertexIndex + 3] = Color.blue;
                    }
                    textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
                }
            }
        }

        public void ChangeRowCountersAmount()
        {
            var rowsCount = CodeField.textComponent.GetTextInfo(CodeField.text).lineCount;
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

        private void LoadNewChallenges()
        {
            var challengesHolder = ChallengesPanel.GetComponentInChildren<VerticalLayoutGroup>().transform;
            for (var i = challengesHolder.transform.childCount; i > 0; i--)
                Destroy(challengesHolder.transform.GetChild(i - 1).gameObject);
            var challengeTexts = gameManager.TaskChallenges[gameManager.CurrentTaskNumber];
            var oneStarChallenge = Instantiate(ChallengePrefab, challengesHolder);
            var twoStarsChallenge = Instantiate(ChallengePrefab, challengesHolder);
            var threeStarsChallenge = Instantiate(ChallengePrefab, challengesHolder);
            oneStarChallenge.GetComponentInChildren<TMP_Text>().text = challengeTexts.OneStarChallenge;
            twoStarsChallenge.GetComponentInChildren<TMP_Text>().text = challengeTexts.TwoStarsChallenge;
            threeStarsChallenge.GetComponentInChildren<TMP_Text>().text = challengeTexts.ThreeStarsChallenge;
        }

        private IEnumerator ShowErrors_COR(List<CompilationError> errors)
        {
            yield return StartCoroutine(TurnTaskIndicatorOn_COR(errorColor));
            var errorsTextField = ErrorsPanel.GetComponentInChildren<TMP_Text>();
            errorsTextField.text = "";
            foreach (var error in errors)
            {
                var errorLine = error.SourceLine - 6;
                var errorColumn = error.SourceColumn <= 6 ? 1 : error.SourceColumn - 6;
                errorsTextField.text += string.Format("({0}, {1}) {2}: {3}\n", errorLine, errorColumn, error.Code, error.Message);
            }              
            if (!isErrorPanelShown)
                ToggleErrorPanelState();
        }

        private IEnumerator ShowExecutingProcess(ScriptProxy proxy)
        {
            var isTaskCompleted = (bool)proxy.Call("isTaskCompleted");
            ExecutingProgressBar.SetActive(true);
            yield return StartCoroutine(PlayAnimation_COR(ExecutingProgressBar, "FillProgressBar"));
            if (isTaskCompleted)
            {
                ExecutingProgressBar.GetComponent<Animator>().Play("ChangeProgressBarColor");
                yield return StartCoroutine(TurnTaskIndicatorOn_COR(successColor));
                onTaskCompleted.Invoke();
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

        private string GetRobotManagementClass(string extraCode)
        {
            return @"
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class RobotManagementClass : MonoBehaviour
{"  
    + CodeField.text
    + extraCode + @"
   public bool isTaskCompleted()
   {" +
       gameManager.GetTests().TestCode + @"
       return totalResult;
   }
}";
        }

        private void Start()
        {
            gameManager = GameManager.Instance;
            rowCountersHolder = RowCounters.transform.GetChild(0).GetChild(0).gameObject;
            LaunchCompiler();
        }
    }
}
