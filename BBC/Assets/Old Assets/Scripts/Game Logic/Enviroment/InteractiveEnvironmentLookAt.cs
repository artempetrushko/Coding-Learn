using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Scripts
{
    public class InteractiveEnvironmentLookAt : MonoBehaviour
    {
        [TextArea]
        [SerializeField] private string description;
        [SerializeField] private UnityEvent onPlayerLookedAt;

        private GameManager gameManager;
        private UIManager uiManager;
        private bool isPlayerClose;

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
                if (Input.GetKeyDown(KeyCode.E))
                {
                    GetComponentInChildren<Camera>().enabled = true;
                    gameManager.Player.GetComponent<PlayerBehaviour>().FreezePlayer();
                    GetComponent<InteractiveItemMarker>().enabled = false;
                    //uiManager.LookAtDescription.text = description;
                    //uiManager.LookAtDescription.gameObject.SetActive(true);

                }
                else if (Input.GetKeyDown(KeyCode.Escape))
                {
                    GetComponentInChildren<Camera>().enabled = false;
                    // uiManager.LookAtDescription.gameObject.SetActive(false);
                    gameManager.Player.GetComponent<PlayerBehaviour>().UnfreezePlayer();
                    //StartCoroutine(uiManager.MakeExitToMenuAvailable_COR());
                    GetComponent<InteractiveItemMarker>().enabled = true;
                    onPlayerLookedAt.Invoke();
                    /*for (var i = 0; i < onPlayerLookedAt.GetPersistentEventCount(); i++)
                        UnityEventTools.RemovePersistentListener(onPlayerLookedAt, i);*/
                }
            }
        }

        private void Start()
        {
            gameManager = GameManager.Instance;
            uiManager = UIManager.Instance;
        }
    }
}
