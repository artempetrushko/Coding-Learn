using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace Scripts
{
    public class InteractivePuzzle : MonoBehaviour
    {
        public string RequiredItemName;
        public bool HasCodingPuzzle;
        public int CodingPuzzleNumber = 0;
        [Space]
        public UnityEvent OnPuzzleSolved;

        private GameManager gameManager;
        private UIManager uiManager;
        private bool isCodingPuzzleStarted = false;
        private bool isPlayerClose = false;
        private bool isPuzzleStarted = false;
        private bool isPadActive = false;

        public void GoToNextPuzzleStep()
        {
            if (!HasCodingPuzzle)
                StartCoroutine(FinishPuzzleByAnimation_COR());
            else StartCodingPuzzle();
        }

        public void FinishPuzzle()
        {
            if (!isPadActive)
                uiManager.InventoryBehaviour.HideInventory_SolvePuzzle();
            else isPadActive = false;
            ReturnToDefaultSceneState();
            GetComponent<SphereCollider>().enabled = true;
            GetComponent<InteractiveItemMarker>().enabled = true;
            uiManager.InventoryBehaviour.InventoryStatement = InventoryStatement.Normal;
        }

        private void StartCodingPuzzle()
        {
            isCodingPuzzleStarted = true;
            isPadActive = true;
            uiManager.InventoryBehaviour.InventoryStatement = InventoryStatement.PuzzleSolving;
            //gameManager.currentTaskNumber = CodingPuzzleNumber;
            uiManager.Canvas.GetComponentInChildren<PadDevelopmentBehaviour>().ShowNewTaskCode();
        }

        public IEnumerator FinishPuzzleByAnimation_COR()
        {
            var usageAnimation = GetComponent<PlayableDirector>();
            if (usageAnimation.playableAsset != null)
            {
                usageAnimation.Play();
                yield return new WaitForSeconds((float)usageAnimation.playableAsset.duration + 1);
            }
            ReturnToDefaultSceneState();
            uiManager.InventoryBehaviour.InventoryStatement = InventoryStatement.Normal;
            OnPuzzleSolved.Invoke();
            gameObject.SetActive(false);
        }

        private void StartPuzzle()
        {
            isPuzzleStarted = true;
            GetComponent<SphereCollider>().enabled = false;
            GetComponent<InteractiveItemMarker>().enabled = false;
            gameManager.Player.GetComponent<PlayerBehaviour>().FreezePlayer();
            gameManager.CurrentInteractivePuzzle = this;
            GetComponentInChildren<Camera>().enabled = true;
            if (!isCodingPuzzleStarted && RequiredItemName != "")
                uiManager.InventoryBehaviour.ShowInventory_SolvePuzzle();
            else StartCodingPuzzle();
        }

        private void ReturnToDefaultSceneState()
        {
            isPuzzleStarted = false;
            gameManager.Player.GetComponent<PlayerBehaviour>().UnfreezePlayer();
            GetComponentInChildren<Camera>().enabled = false;
            //StartCoroutine(uiManager.MakeExitToMenuAvailable_COR());
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == gameManager.Player)
            {
                GetComponent<InteractiveItemMarker>().enabled = true;
                isPlayerClose = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == gameManager.Player)
            {
                GetComponent<InteractiveItemMarker>().enabled = false;
                isPlayerClose = false;
            }
        }

        private void Update()
        {
            if (isPlayerClose)
            {
                if (Input.GetKeyDown(KeyCode.E) && !isPuzzleStarted)
                    StartPuzzle();
                else if (Input.GetKeyDown(KeyCode.Escape) && isPuzzleStarted && !isCodingPuzzleStarted)
                    FinishPuzzle();
            }
        }

        private void Start()
        {
            gameManager = GameManager.Instance;
            uiManager = UIManager.Instance;
        }
    }
}