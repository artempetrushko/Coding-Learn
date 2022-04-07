using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts
{
    public class LevelStatsCardBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            gameObject.GetComponent<Animator>().Play("ShowStarsCounter");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            gameObject.GetComponent<Animator>().Play("HideStarsCounter");
        }
    }
}
