using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.Localization.Components;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class MainMenuSectionController 
    {
        private MainMenuSectionView _mainMenuSectionView;

        [SerializeField]
        private Button mainMenuButtonPrefab;

        public MainMenuSectionController(MainMenuSectionView mainMenuSectionView)
        {
            _mainMenuSectionView = mainMenuSectionView;
        }

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

        public async UniTask ShowContentAsync() => await ChangeMainMenuVisibilityAsync(true);

        public async UniTask HideContentAsync() => await ChangeMainMenuVisibilityAsync(false);

        public async UniTask PlayStartAnimationAsync()
        {
            await HideBlackScreenAsync();
            await ChangeMainMenuVisibilityAsync(true);
        }



        [SerializeField]
        private GameObject content;
        [SerializeField]
        private List<Image> backgroundParts = new();
        [SerializeField]
        private Image blackScreen;

        private Sequence mainMenuShowingTween;

        public async UniTask HideBlackScreenAsync()
        {
            blackScreen.gameObject.SetActive(true);
            await blackScreen
                .DOColor(new Color(0, 0, 0, 0), 2f)
                .AsyncWaitForCompletion();
            blackScreen.gameObject.SetActive(false);
        }

        public async UniTask ChangeMainMenuVisibilityAsync(bool isVisible)
        {
            mainMenuShowingTween ??= CreateMainMenuShowingTween();
            if (isVisible)
            {
                _mainMenuSectionView.SetContentLocalPosition(new Vector3(0, content.GetComponent<RectTransform>().rect.height, 0));
                mainMenuShowingTween.PlayForward();
            }
            else
            {
                mainMenuShowingTween.PlayBackwards();
            }
            await mainMenuShowingTween.AsyncWaitForRewind();
        }

        private Sequence CreateMainMenuShowingTween()
        {
            var backgroundFillingTotalDuration = 1.5f;
            var backgroundPartFillingDuration = backgroundFillingTotalDuration / backgroundParts.Count;
            _mainMenuSectionView.SetContentLocalPosition(new Vector3(0, content.GetComponent<RectTransform>().rect.height, 0));

            var tweenSequence = DOTween.Sequence();
            tweenSequence.Pause();
            backgroundParts.ForEach(part => tweenSequence.Append(DOTween.To(x => part.fillAmount = x, 0f, 1f, backgroundPartFillingDuration)));
            tweenSequence.Append(content.transform.DOLocalMoveY(0f, 0.5f));
            tweenSequence.SetAutoKill(false);
            return tweenSequence;
        }
    }
}
