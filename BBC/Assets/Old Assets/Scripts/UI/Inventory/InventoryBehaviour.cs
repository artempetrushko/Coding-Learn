using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public enum InventoryStatement
    {
        Normal,
        PuzzleSolving
    }

    public class InventoryBehaviour : MonoBehaviour
    {
        public GameObject Inventory;
        public Text ItemName;
        public Text ItemDescription;

        [HideInInspector] public InventoryStatement InventoryStatement = InventoryStatement.Normal;
        [HideInInspector] public bool IsOpen;

        [SerializeField] private GameObject scriptInventoryItems;
        [SerializeField] private GameObject otherInventoryItems;
        [SerializeField] private GameObject notes;
        [SerializeField] private GameObject inventoryItemPrefab;

        private GameManager gameManager;
        private UIManager uiManager;
        private GameObject lastOpenedCategory;

        public void ShowInventory_SolvePuzzle()
        {
            ShowInventory();
            InventoryStatement = InventoryStatement.PuzzleSolving;
        }

        public void HideInventory_SolvePuzzle()
        {
            HideInventory();
            InventoryStatement = InventoryStatement.Normal;
        }

        public void ShowInventoryCategory(GameObject category)
        {
            if (category != lastOpenedCategory)
            {
                category.SetActive(true);
                lastOpenedCategory.SetActive(false);
                lastOpenedCategory = category;
            }
        }

        private void ShowInventory()
        {
            IsOpen = true;
            gameManager.Player.GetComponent<PlayerBehaviour>().FreezePlayer();
            UpdateInventory();
            otherInventoryItems.SetActive(false);
            notes.SetActive(false);
            lastOpenedCategory = scriptInventoryItems;
            Inventory.GetComponent<Animator>().Play("ShowInventory");
        }

        private void HideInventory()
        {
            IsOpen = false;
            ClearInventory();
            Inventory.GetComponent<Animator>().Play("HideInventory");
            gameManager.Player.GetComponent<PlayerBehaviour>().UnfreezePlayer();
            //StartCoroutine(uiManager.MakeExitToMenuAvailable_COR());
        }

        private void UpdateInventory()
        {
            ClearInventory();
            FillInventory(gameManager.ScriptItems, scriptInventoryItems);
            FillInventory(gameManager.OtherItems, otherInventoryItems);
            FillInventory(gameManager.Notes, notes);
        }

        private void ClearInventory()
        {
            ClearCategory(scriptInventoryItems);
            ClearCategory(otherInventoryItems);
            ClearCategory(notes);
        }

        private void FillInventory(List<InteractiveItem> items, GameObject itemsContainer)
        {
            for (var i = items.Count - 1; i >= 0; i--)
            {
                if (items[i].Count <= 0)
                {
                    items.RemoveAt(i);
                    continue;
                }
                var newItem = Instantiate(inventoryItemPrefab, itemsContainer.transform.GetChild(0).GetChild(0));
                var itemComponent = newItem.GetComponent<InventoryItem>();
                itemComponent.ItemReference = items[i];
                newItem.transform.GetChild(0).GetComponent<Image>().sprite = itemComponent.ItemReference.Icon;
                newItem.transform.GetChild(1).GetComponent<Text>().text = itemComponent.ItemReference.Count > 1 ? itemComponent.ItemReference.Count.ToString() : "";
            }
        }

        private void ClearCategory(GameObject items)
        {
            var scriptItemsContainer = items.transform.GetChild(0).GetChild(0);
            for (var i = scriptItemsContainer.childCount - 1; i >= 0; i--)
                Destroy(scriptItemsContainer.GetChild(i).gameObject);
        }

        private void Update()
        {
            if (InventoryStatement == InventoryStatement.Normal)
            {
                if (Input.GetKeyDown(KeyCode.I))
                {
                    if (!IsOpen)
                        ShowInventory();
                    else HideInventory();
                }
                else if (Input.GetKeyDown(KeyCode.Escape) && IsOpen)
                    HideInventory();
            }
        }

        private void Start()
        {
            gameManager = GameManager.Instance;
            uiManager = UIManager.Instance;
        }
    }
}
