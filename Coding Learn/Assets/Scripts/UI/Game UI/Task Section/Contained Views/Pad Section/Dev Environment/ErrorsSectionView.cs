using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class ErrorsSectionView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text errorsText;
        [SerializeField]
        private Scrollbar scrollbar;
        [Space, SerializeField]
        private ErrorsSectionAnimator animator;

        private bool isVisible = false;

        public async UniTask ChangeVisibilityAsync(bool isVisible)
        {
            if (this.isVisible != isVisible)
            {
                this.isVisible = isVisible;
                await animator.ChangeVisibilityAsync(isVisible);
            }          
        }

        public void ToggleVisibility()
        {
            isVisible = !isVisible;
            animator.ChangeVisibilityAsync(isVisible);
        }

        public void SetContent(string errorsMessage)
        {
            errorsText.text = errorsMessage;
            scrollbar.value = 1;
        }
    }
}
