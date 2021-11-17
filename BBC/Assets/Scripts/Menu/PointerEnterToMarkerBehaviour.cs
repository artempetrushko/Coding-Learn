using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts
{
    public class PointerEnterToMarkerBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            gameObject.transform.parent.GetChild(1).GetComponent<Animator>().Play("DrawLevelDescription");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            gameObject.transform.parent.GetChild(1).GetComponent<Animator>().Play("EraseLevelDescription");
        }
    }
}
