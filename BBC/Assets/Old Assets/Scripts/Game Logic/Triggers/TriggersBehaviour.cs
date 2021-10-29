using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggersBehaviour : MonoBehaviour
{
    [Header("Триггеры заданий")]
    public GameObject TaskTriggers;
    [Header("Триггеры входа в помещение")]
    public GameObject EnterTriggers;
    [Header("Триггеры смены сцены")]
    public GameObject ChangeSceneTriggers;
    [Header("Триггеры активации сценарного момента")]
    public GameObject ScenarioTriggers;
    [Header("Триггер завершения уровня")]
    public GameObject FinishTrigger;
    [Header("Диалоговые персонажи с триггерами")]
    public GameObject DialogCharacters;

    private UIManager uiManager;
    private GameManager gameManager;

    public void ActivateTrigger_Any(GameObject trigger) => ActivateTrigger(trigger);

    public void ActivateTrigger_Task(int triggerNumber) => ActivateTrigger(TaskTriggers.transform.GetChild(triggerNumber - 1).gameObject);

    public void ActivateTrigger_ScriptMoment(int triggerNumber) => ActivateTrigger(ScenarioTriggers.transform.GetChild(triggerNumber - 1).gameObject);

    public void ActivateTrigger_EnterToMiniScene(int triggerNumber) => ActivateTrigger(EnterTriggers.transform.GetChild(triggerNumber - 1).gameObject);

    public void ActivateTrigger_ChangeScene(int triggerNumber) => ActivateTrigger(ChangeSceneTriggers.transform.GetChild(triggerNumber - 1).gameObject);

    public void ActivateTrigger_Dialogue(int npcOrderNumber) => ActivateTrigger(DialogCharacters.transform.GetChild(npcOrderNumber - 1).GetChild(1).gameObject);

    public void ActivateTrigger_Finish() => ActivateTrigger(FinishTrigger);

    private void ActivateTrigger(GameObject trigger)
    {
        trigger.SetActive(true);
        trigger.GetComponent<TargetWaypoint>().Waypoint.gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        var triggerData = other.gameObject.GetComponent<TriggerData>();
        if (triggerData != null)
        {
            switch (triggerData.TriggerPurpose)
            {
                case TriggerData.Purpose.Task:
                    if (!gameManager.HasTasksCompleted[triggerData.Task_TaskNumber - 1])
                        gameManager.CurrentTaskNumber = triggerData.Task_TaskNumber;
                    break;
            }
            uiManager.ActionButtonBehaviour.ActivatedTrigger = triggerData;
            uiManager.ActionButtonBehaviour.ActivateButton(triggerData.ActionButtonText);
        }
        else if (other.name.StartsWith("Coin"))
            StartCoroutine(PickCoinUp_COR(other));
    }

    private void OnTriggerExit(Collider other)
    {
        if (uiManager.ActionButton.IsActive() && other.GetComponent<TriggerData>() != null)
            StartCoroutine(uiManager.ActionButtonBehaviour.DeleteActionButton_COR());
    }
 
    private IEnumerator PickCoinUp_COR(Collider coin)
    {
        coin.GetComponentInChildren<Animator>().Play("PickCoinUp");
        yield return new WaitForSeconds(1f);
        coin.gameObject.SetActive(false);
        gameManager.CoinsCount++;
        uiManager.PadMenuBehaviour.UpdatePadData();
    }
    
    void Start()
    {
        uiManager = UIManager.Instance;
        gameManager = GameManager.Instance;
        uiManager.ActionButton.gameObject.SetActive(false);
    } 
}
