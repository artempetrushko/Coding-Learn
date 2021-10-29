using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PuzzleCompletingActions : MonoBehaviour
{
    private GameManager gameManager;

    #region Общие действия
    public void OpenDoor(GameObject door)
    {
        door.GetComponent<Rigidbody>().isKinematic = false;
    }

    public void EnableObject(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }

    public void DisableObject(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
    #endregion

    private void Start()
    {
        gameManager = GameManager.Instance;
    }
}
