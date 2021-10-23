using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Note : MonoBehaviour
{
    [Header("Содержание записки")]
    public string Title;
    [TextArea]
    public string Description;
    [Space]
    [Tooltip("Иконка для отображения в инвентаре")]
    public Sprite Icon;
    public bool canPickUp;
    [Space]
    public UnityEvent OnItemPickedUp;
    public UnityEvent OnPlayerLookedAt;

    private GameManager gameManager;
    private UIManager uiManager;
    private bool isPlayerClose = false;
    private bool isShown = false;

    private void PutItemInInventory()
    {
        gameManager.Notes.Add(new InteractiveItem()
        {
            Name = Title,
            Description = Description,
            Count = 1,
            Icon = Icon,
            Type = ItemType.Note
        });
        OnItemPickedUp.Invoke();
        Destroy(gameObject.transform.parent.gameObject);
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
            if (Input.GetKeyDown(KeyCode.E) && !isShown)
            {
                isShown = true;
                GetComponent<InteractiveItemMarker>().enabled = false;
                gameManager.Player.GetComponent<PlayerBehaviour>().FreezePlayer();
                uiManager.isExitToMenuAvailable = false;
                uiManager.NoteReadingPanelBehaviour.Title.text = Title;
                uiManager.NoteReadingPanelBehaviour.Description.text = Description;
                uiManager.NoteReadingPanelBehaviour.GetComponent<Animator>().Play("ShowNote");
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && isShown)
            {
                isShown = false;
                gameManager.Player.GetComponent<PlayerBehaviour>().UnfreezePlayer();
                StartCoroutine(uiManager.MakeExitToMenuAvailable_COR());
                uiManager.NoteReadingPanelBehaviour.GetComponent<Animator>().Play("HideNote");
                GetComponent<InteractiveItemMarker>().enabled = true;
                OnPlayerLookedAt.Invoke();
                OnPlayerLookedAt.RemoveAllListeners();
                if (canPickUp)
                    PutItemInInventory();
            }
        }
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        uiManager = UIManager.Instance;
    }
}
