using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts
{
    public class TipBehaviour : MonoBehaviour
    {
        public UnityEvent OnTipEnable;
        public UnityEvent OnTipDisable;

        public void ShowTip()
        {
            OnTipEnable.Invoke();
            var animator = GetComponentInChildren<Animator>();
            if (animator != null)
                animator.Play("ShowAside");
        }

        public void ShowNextTip(GameObject nextTip)
        {
            nextTip.SetActive(true);
            nextTip.GetComponent<TipBehaviour>().ShowTip();
            CloseTip();
        }

        public void CloseTip()
        {
            OnTipDisable.Invoke();
            gameObject.SetActive(false);
        }
    }
}
