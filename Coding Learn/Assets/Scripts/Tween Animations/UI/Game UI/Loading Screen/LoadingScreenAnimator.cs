using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class LoadingScreenAnimator : MonoBehaviour
    {
        [SerializeField]
        private Image background;

        public async UniTask ShowBackgroundAsync()
        {
            await background
                .DOFade(1f, 1.5f);
                //.AsyncWaitForCompletion();    
        }
    }
}
