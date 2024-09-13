using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Game
{
    public class ErrorsSectionView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _errorsText;
        [SerializeField] private Scrollbar _scrollbar;

        private bool _isVisible = false;

        public void SetErrorsText(string text) => _errorsText.text = text;

        public void SetScrollbarValue(float value) => _scrollbar.value = value;

        public async UniTask SetVisibilityAsync(bool isVisible)
        {
            if (_isVisible != isVisible)
            {
                _isVisible = isVisible;

                var movementSign = isVisible ? 1 : -1;
                await transform
                    .DOLocalMoveY(transform.localPosition.y + transform.GetComponent<RectTransform>().sizeDelta.y * movementSign, 1.5f)
                    .AsyncWaitForCompletion();
            }
        }
    }
}
