using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class DialogActions : MonoBehaviour
    {
        public VIDE_Assign CurrentNPC;

        private GameManager gameManager;
        private UIManager uiManager;
        private TriggersBehaviour triggersBehaviour;

        public void StartDialog(VIDE_Assign currentNPC)
        {
            CurrentNPC = currentNPC;
            gameObject.GetComponent<VIDEUIManager1>().Interact(CurrentNPC);
        }

        public void ActivateDialogCamera_Animated(int cameraNumber)
        {
            ActivateDialogCamera_Static(cameraNumber);
            var characterName = CurrentNPC.alias;
            gameManager.CurrentDialogCamera.GetComponent<Animator>().Play("MoveDialogCamera_" + characterName + "_" + cameraNumber);
        }

        public void ActivateDialogCamera_Static(int cameraNumber)
        {
            var dialogCameras = CurrentNPC.transform.parent.GetChild(2);
            var newDialogCamera = dialogCameras.GetChild(cameraNumber - 1).gameObject.GetComponent<Camera>();
            newDialogCamera.enabled = true;
            if (gameManager.CurrentDialogCamera != null)
                gameManager.CurrentDialogCamera.enabled = false;
            gameManager.CurrentDialogCamera = newDialogCamera;
        }

        public void ActivateNpcAnimation(string animationName) => CurrentNPC.gameObject.transform.parent.GetComponentInChildren<Animator>().Play(animationName);

        public void ActivateTask_PostDialog(int taskNumber)
        {
            gameManager.CurrentTaskNumber = taskNumber;
            StartCoroutine(uiManager.ActionButtonBehaviour.ActivateTask_COR(false));
        }

        public void ActivateTrigger_ChangeScene(int triggerNumber) => triggersBehaviour.ActivateTrigger_ChangeScene(triggerNumber);

        public void ActivateTrigger_NPC(int npcOrderNumber) => triggersBehaviour.ActivateTrigger_Dialogue(npcOrderNumber);

        public void ActivateTrigger_Task(int triggerNumber) => triggersBehaviour.ActivateTrigger_Task(triggerNumber);

        public void DeactivateDialogCamera() => gameManager.CurrentDialogCamera.enabled = false;

        public void LeaveNPC()
        {
            CurrentNPC.gameObject.SetActive(false);
            StartCoroutine(uiManager.ActionButtonBehaviour.DeleteActionButton_COR());
            uiManager.ActionButtonBehaviour.IsPressed = false;
        }

        private void Start()
        {
            gameManager = GameManager.Instance;
            uiManager = UIManager.Instance;
            //triggersBehaviour = gameManager.Player.GetComponentInChildren<TriggersBehaviour>();
        }
    }
}
