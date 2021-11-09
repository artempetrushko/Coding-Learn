using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Scripts
{
    public class ScriptTrigger : MonoBehaviour
    {
        [Tooltip("Выберите, если это первая катсцена на уровне")]
        [SerializeField] private bool isFirstCutscene;
        [SerializeField] private int taskNumber;
        [SerializeField] private PlayableAsset startCutscene;
        [SerializeField] private PlayableAsset taskEndingCutscene;
        [SerializeField] private PlayableAsset transitionToNextTaskCutscene;

        private GameManager gameManager;
        private UIManager uiManager;

        public void MakeTransitionToNextTask() => StartCoroutine(MakeTransitionToNextTask_COR());

        private IEnumerator MakeTransitionToNextTask_COR()
        {
            yield return StartCoroutine(PlayTimeline_COR(taskEndingCutscene));
            yield return StartCoroutine(PlayCutsceneAndStartTask_COR(transitionToNextTaskCutscene));
        }

        private IEnumerator PlayCutsceneAndStartTask_COR(PlayableAsset timeline)
        {
            yield return StartCoroutine(PlayTimeline_COR(timeline));
            uiManager.TaskPanelBehaviour.StartNewTask();
        }

        private IEnumerator PlayTimeline_COR(PlayableAsset timeline)
        {
            var playableDirector = gameObject.GetComponent<PlayableDirector>();
            playableDirector.playableAsset = timeline;
            playableDirector.Play();
            yield return new WaitForSeconds((float)timeline.duration);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == gameManager.Player)
            {
                gameManager.CurrentTaskNumber = taskNumber;
                gameManager.CurrentScriptTrigger = this;
            }
        }

        private void Start()
        {
            gameManager = GameManager.Instance;
            uiManager = UIManager.Instance;
            if (isFirstCutscene)
                StartCoroutine(PlayCutsceneAndStartTask_COR(startCutscene));
        }
    }
}
