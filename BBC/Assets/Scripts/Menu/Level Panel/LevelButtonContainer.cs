using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts
{
    public class LevelButtonContainer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private Button levelButton;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (GetComponentInChildren<Button>().interactable)
            {
                transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
                transform.parent.GetChild(0).GetComponent<Animator>().Play("ShowLevelDescription");
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (GetComponentInChildren<Button>().interactable)
            {
                transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);
                transform.parent.GetChild(0).GetComponent<Animator>().Play("HideLevelDescription");
            }
        }

        private void OnEnable()
        {
            levelButton = GetComponentInChildren<Button>();
        }
    }
}
