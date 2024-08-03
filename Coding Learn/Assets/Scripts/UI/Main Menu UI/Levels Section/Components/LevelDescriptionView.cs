using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class LevelDescriptionView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private Image[] _backgroundParts;

        public void SetDescriptionText(string text) => _descriptionText.text = text;

        private void OnEnable()
        {
            ChangeVisibilityAsync(true).Forget();
        }

        public async UniTask ChangeVisibilityAsync(bool isVisible)
        {
            var backgroundShowingTime = 0.3f;
            foreach (var part in _backgroundParts)
            {
                _ = part.DOFillAmount(isVisible ? 1f : 0f, backgroundShowingTime);
            }
            await UniTask.WaitForSeconds(backgroundShowingTime);
            _ = _descriptionText.DOFade(1, 0.15f);
        }
    }
}
