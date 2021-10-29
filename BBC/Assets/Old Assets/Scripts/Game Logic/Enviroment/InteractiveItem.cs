using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public enum ItemType
{
    Scripting,
    Other,
    Note
}

public class InteractiveItem : MonoBehaviour
{
    [Header("Информация о предмете")]
    public string Name;
    public string Description;
    public int Count = 1;
    [Tooltip("Иконка для отображения в инвентаре")]
    public Sprite Icon;
    [Tooltip("Тип предмета: сюжетный или прочее")]
    public ItemType Type;
    [Space]
    public UnityEvent OnItemPickedUp;

    private GameManager gameManager;
    private bool isPlayerClose = false;

    private void CheckItemType()
    {
        List<InteractiveItem> itemsList;
        switch (Type)
        {
            case ItemType.Scripting:
                itemsList = gameManager.ScriptItems;
                break;
            default:
                itemsList = gameManager.OtherItems;
                break;
        }
        PutItemInInventory(itemsList);
    }

    private void PutItemInInventory(List<InteractiveItem> items)
    {
        var sameItem = items.Where(x => x.Name == Name);
        if (sameItem.Count() != 0)
            sameItem.First().Count += Count;
        else
        {
            var itemCopy = new InteractiveItem();
            foreach (var field in typeof(InteractiveItem).GetFields())
                field.SetValue(itemCopy, field.GetValue(this));
            items.Add(itemCopy);
        }
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
        if (isPlayerClose && Input.GetKeyDown(KeyCode.E))
            CheckItemType();
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
    }
}
