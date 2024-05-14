using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace Scripts
{
    public class MainMenuSectionView : MonoBehaviour
    {
        [SerializeField]
        private GameObject buttonsContainer;
        [SerializeField]
        private Button mainMenuButtonPrefab;
        [Space, SerializeField]
        private MainMenuSectionAnimator animator;

        public void CreateButtons(List<MainMenuButtonData> buttonDatas)
        {
            for (var i = 0; i < buttonDatas.Count; i++)
            {
                var newButton = Instantiate(mainMenuButtonPrefab, buttonsContainer.transform);
                var currentIndex = i;
                var buttonLocalizedString = buttonDatas[currentIndex].LocalizedString;
                newButton.GetComponentInChildren<LocalizeStringEvent>().StringReference.SetReference(buttonLocalizedString.TableReference, buttonLocalizedString.TableEntryReference);
                newButton.onClick.AddListener(buttonDatas[currentIndex].onButtonPressed.Invoke);
            }
        }

        public async UniTask ShowContentAsync() => await animator.ChangeMainMenuVisibilityAsync(true);

        public async UniTask HideContentAsync() => await animator.ChangeMainMenuVisibilityAsync(false);

        public async UniTask PlayStartAnimationAsync()
        {
            await animator.HideBlackScreenAsync();
            await animator.ChangeMainMenuVisibilityAsync(true);
        }
    }
}
