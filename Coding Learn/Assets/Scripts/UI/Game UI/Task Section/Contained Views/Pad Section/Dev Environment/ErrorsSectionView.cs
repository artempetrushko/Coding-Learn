using Cysharp.Threading.Tasks;
using DG.Tweening;
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

        private bool isVisible = false;

        public async UniTask ChangeVisibilityAsync(bool isVisible)
        {
            if (this.isVisible != isVisible)
            {
                this.isVisible = isVisible;

                var movementSign = isVisible ? 1 : -1;
                await transform
                    .DOLocalMoveY(transform.localPosition.y + transform.GetComponent<RectTransform>().sizeDelta.y * movementSign, 1.5f)
                    .AsyncWaitForCompletion();
            }          
        }

        public void SetContent(string errorsMessage)
        {
            errorsText.text = errorsMessage;
            scrollbar.value = 1;
        }
    }
}
