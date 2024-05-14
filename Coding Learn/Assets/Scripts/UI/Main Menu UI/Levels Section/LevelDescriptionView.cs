using TMPro;
using UnityEngine;

namespace Scripts
{
    public class LevelDescriptionView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text descriptionText;
        [Space, SerializeField]
        private LevelDescriptionAnimator animator;

        public void SetContent(string levelDescription)
        {
            descriptionText.text = levelDescription;
        }

        private void OnEnable()
        {
            _ = animator.ChangeVisibilityAsync(true);
        }
    }
}
