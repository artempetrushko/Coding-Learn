using UnityEngine;

namespace Scripts
{
    public class PadModalWindow : MonoBehaviour
    {
        [SerializeField]
        protected PadModalWindowAnimator animator;

        public virtual void SetVisibility(bool isVisible) => animator.ChangeVisibilityAsync(isVisible);
    }
}
