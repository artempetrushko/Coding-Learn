using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class PadModalWindow : MonoBehaviour
    {
        [SerializeField]
        protected PadModalWindowAnimator animator;

        public virtual void SetVisibility(bool isVisible) => StartCoroutine(animator.ChangeVisibility_COR(isVisible));
    }
}
