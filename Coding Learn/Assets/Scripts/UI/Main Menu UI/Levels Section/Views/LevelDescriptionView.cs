using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class LevelDescriptionView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text descriptionText;

        public void SetContent(string levelDescription)
        {
            descriptionText.text = levelDescription;
        }

        private void OnEnable()
        {
            _ = ChangeVisibilityAsync(true);
        }



        [SerializeField]
        private Image[] backgroundParts;

        public async UniTask ChangeVisibilityAsync(bool isVisible)
        {
            var backgroundShowingTime = 0.3f;
            foreach (var part in backgroundParts)
            {
                _ = part.DOFillAmount(isVisible ? 1f : 0f, backgroundShowingTime);
            }
            await UniTask.WaitForSeconds(backgroundShowingTime);
            _ = descriptionText.DOFade(1, 0.15f);
        }
    }
}
