using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class LoadingScreenView : MonoBehaviour
    {
        [SerializeField]
        private LoadingBar loadingBar;
        [SerializeField]
        private Image background;
        [Space, SerializeField]
        private LoadingScreenAnimator animator;

        public async UniTask ShowAsync()
        {
            await animator.ShowBackgroundAsync();
            loadingBar.gameObject.SetActive(true);
        }

        public void SetBackground(Sprite backgroundImage)
        {
            background.sprite = backgroundImage;
        }

        public void SetLoadingBarState(float loadingProgress) => loadingBar.SetInfo(loadingProgress);
    }
}
