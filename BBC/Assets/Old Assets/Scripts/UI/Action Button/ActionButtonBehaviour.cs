using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using UnityEngine.Events;

namespace Scripts
{
    public class ActionButtonBehaviour : MonoBehaviour
    {
        public GameObject ActionButtonBackground;
        public TriggerData ActivatedTrigger;
        public bool IsPressed;

        [Space] public UnityEvent OnTaskCalled;
        [Space] public UnityEvent<VIDE_Assign> OnDialogStarted;
        [Space] public UnityEvent OnLevelFinished;

        private PlayerBehaviour robotBehaviour;
        private TriggersBehaviour triggersBehaviour;
        private UIManager uiManager;
        private GameManager gameManager;

        public void MakeAction() => StartCoroutine(MakeAction_COR());

        #region Активация/деактивация кнопки
        public void ActivateButton(string buttonText)
        {
            if (!gameObject.GetComponent<Button>().IsActive())
            {
                gameObject.SetActive(true);
                StartCoroutine(ShowActionButton_COR());
            }
            gameObject.GetComponentInChildren<Text>().text = buttonText;
        }

        public IEnumerator ShowActionButton_COR()
        {
            ActionButtonBackground.GetComponent<Animator>().Play("DrawBackground");
            yield return new WaitForSeconds(0.15f);
            gameObject.GetComponent<Animator>().Play("ScaleInterfaceUp");
            yield return new WaitForSeconds(0.15f);
        }

        public IEnumerator DeleteActionButton_COR()
        {
            yield return StartCoroutine(HideActionButton_COR());
            gameObject.SetActive(false);
        }

        public IEnumerator HideActionButton_COR()
        {
            gameObject.GetComponent<Animator>().Play("CollapseInterface");
            yield return new WaitForSeconds(0.15f);
            ActionButtonBackground.GetComponent<Animator>().Play("EraseBackground");
            yield return new WaitForSeconds(0.15f);
        }
        #endregion

        public IEnumerator ActivateTask_COR(bool hasActivateButton = true)
        {
            var currentTaskNumber = gameManager.GetCurrentTaskNumber();
            //gameManager.IsTaskStarted = true;
            uiManager.PadMenuBehaviour.ShowIDEButton.interactable = true;
            robotBehaviour.FreezePlayer();
            if (gameManager.SceneIndex != 0)
            {
                ActivatedTrigger.gameObject.SetActive(false);
                yield return StartCoroutine(TurnOnTaskCamera_COR(currentTaskNumber, hasActivateButton));
            }
            OnTaskCalled.Invoke();
        }

        public void ActivateScenarioMoment()
        {
            switch (gameManager.SceneIndex)
            {
                case 3:
                    StartCoroutine(ActivateScenarioMoment_Level_3_COR(ActivatedTrigger.ScriptMoment_TriggerNumber));
                    break;
            }
            ActivatedTrigger.gameObject.SetActive(false);
        }

        public void FinishLevel() => StartCoroutine(FinishLevel_COR());

        private IEnumerator MakeAction_COR()
        {
            if (uiManager.PadMenuBehaviour.IsPadCalled)
            {
                uiManager.PadMenuBehaviour.Pad.GetComponent<Animator>().Play("MoveRight_Pad");
                yield return new WaitForSeconds(0.667f);
            }
            ActivatedTrigger.GetComponent<TargetWaypoint>().Waypoint.gameObject.SetActive(false);
            switch (ActivatedTrigger.TriggerPurpose)
            {
                case TriggerData.Purpose.Task:
                    StartCoroutine(ActivateTask_COR());
                    yield break;

                case TriggerData.Purpose.EnterToMiniScene:
                    StartCoroutine(EnterToMiniScene_COR());
                    yield break;

                case TriggerData.Purpose.ScriptMoment:
                    ActivateScenarioMoment();
                    yield break;

                case TriggerData.Purpose.SaveGame:
                    StartCoroutine(SaveGame_COR());
                    yield break;

                case TriggerData.Purpose.Dialog:
                    StartCoroutine(ActivateDialog_COR());
                    yield break;

                case TriggerData.Purpose.ChangeLevel:
                    StartCoroutine(ChangeLevel_COR());
                    yield break;

                case TriggerData.Purpose.FinishLevel:
                    FinishLevel();
                    yield break;
            }
        }

        private IEnumerator ActivateDialog_COR()
        {
            ActivatedTrigger.transform.GetChild(0).gameObject.SetActive(false);
            yield return StartCoroutine(HideActionButton_COR());
            var dialogueNPC = gameManager.Player.GetComponent<VIDEDemoPlayer>().inTrigger;
            OnDialogStarted.Invoke(dialogueNPC);
        }

        private IEnumerator ChangeLevel_COR()
        {
            uiManager.BlackScreen.transform.localScale = new Vector3(1, 1, 1);
            uiManager.BlackScreen.GetComponentInChildren<Animator>().Play("AppearBlackScreen");
            yield return new WaitForSeconds(1.4f);
            SceneManager.LoadScene(ActivatedTrigger.ChangeLevel_NextLevelIndex);
        }

        private IEnumerator SaveGame_COR()
        {
            uiManager.ActionButton.interactable = false;
            ActivatedTrigger.GetComponentInChildren<Animator>().Play("Saving");
            var buttonText = uiManager.ActionButton.GetComponentInChildren<Text>();
            for (var i = 1; i <= 3; i++)
            {
                buttonText.text = "Сохранение" + new string('.', i);
                yield return new WaitForSeconds(0.667f);
            }
            buttonText.text = "Игра сохранена.";
            yield return new WaitForSeconds(1.5f);
            buttonText.text = "Сохранить игру";
            uiManager.ActionButton.interactable = true;
        }

        private IEnumerator TurnOnTaskCamera_COR(int currentTaskNumber, bool hasActivateButton)
        {
            if (hasActivateButton)
                yield return StartCoroutine(HideActionButton_COR());
            gameManager.CurrentSceneCamera.GetComponent<PlayableDirector>().playableAsset = Resources.Load<PlayableAsset>("Timelines/Tasks/Level " + gameManager.SceneIndex + "/MoveToTask_" + currentTaskNumber);
            gameManager.CurrentSceneCamera.GetComponent<PlayableDirector>().Play();
            yield return new WaitForSeconds(2f);
        }

        private IEnumerator EnterToMiniScene_COR()
        {
            robotBehaviour.FreezePlayer();
            ActivatedTrigger.gameObject.SetActive(false);
            var blackScreen = uiManager.BlackScreen.transform.GetChild(0).gameObject;
            uiManager.BlackScreen.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            blackScreen.GetComponent<Animator>().Play("AppearBlackScreen");
            yield return new WaitForSeconds(2.5f);
            gameManager.Player.transform.position = ActivatedTrigger.EnterToMiniScene_DestinationTrigger.transform.position;
            ActivatedTrigger.gameObject.SetActive(true);
            blackScreen.GetComponent<Animator>().Play("HideBlackScreen");
            yield return new WaitForSeconds(2f);
            uiManager.BlackScreen.transform.localScale = new Vector3(0, 0, 0);
            robotBehaviour.UnfreezePlayer();
        }

        private IEnumerator FinishLevel_COR()
        {
            yield return StartCoroutine(HideActionButton_COR());
            OnLevelFinished.Invoke();
        }

        private IEnumerator ActivateScenarioMoment_Level_3_COR(int triggerNumber)
        {
            yield return StartCoroutine(HideActionButton_COR());
            /*switch (triggerNumber)
            {
                case 1:
                    var message = gameManager.ScenarioMessages[0];
                    uiManager.ExtendedTaskPanelBehaviour.ExtendedTaskTitle.text = message.Title;
                    uiManager.ExtendedTaskPanelBehaviour.ExtendedTaskDescription.text = message.Description;
                    uiManager.ExtendedTaskPanelBehaviour.IsTaskMessage = false;
                    uiManager.ExtendedTaskPanelBehaviour.OpenTaskExtendedDescription_Special();
                    gameManager.Target = "Найти ключи, чтобы открыть ворота (0/3)";
                    triggersBehaviour.ActivateTrigger_Task(8);
                    triggersBehaviour.ActivateTrigger_EnterToMiniScene(1);
                    triggersBehaviour.ActivateTrigger_EnterToMiniScene(3);
                    break;
                case 2:
                    var playableDirector = gameManager.CurrentSceneCamera.GetComponent<PlayableDirector>();
                    playableDirector.playableAsset = Resources.Load<PlayableAsset>("Timelines/Cutscenes/Level 3/OpenGates");
                    playableDirector.Play();
                    yield return new WaitForSeconds((float)playableDirector.playableAsset.duration);
                    robotBehaviour.UnfreezePlayer();
                    triggersBehaviour.ActivateTrigger_Finish();
                    break;
            }*/
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E) && ActivatedTrigger.gameObject.activeInHierarchy)
            {
                if (uiManager.ActionButton.IsActive() && !IsPressed)
                {
                    IsPressed = true;
                    StartCoroutine(MakeAction_COR());
                }
                else if (gameManager.Player.GetComponent<VIDEDemoPlayer>().inTrigger)
                    uiManager.Canvas.GetComponentInChildren<VIDEUIManager1>().Interact(gameManager.Player.GetComponent<VIDEDemoPlayer>().inTrigger);
            }
        }

        private void Awake()
        {
            gameManager = GameManager.Instance;
            robotBehaviour = gameManager.Player.GetComponent<PlayerBehaviour>();
            triggersBehaviour = gameManager.Player.GetComponentInChildren<TriggersBehaviour>();
            uiManager = UIManager.Instance;
        }
    }
}
