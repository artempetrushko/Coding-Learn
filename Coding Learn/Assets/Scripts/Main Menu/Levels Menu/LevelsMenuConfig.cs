using UnityEngine;

namespace UI.MainMenu
{
    [CreateAssetMenu(fileName = "Level Section Config", menuName = "Game Configs/Level Section Config")]
    public class LevelsMenuConfig : ScriptableObject
    {
        [SerializeField] private float _thumbnailChangingDuration = 1f;
        [SerializeField] private float _levelDescriptionVisibilityChangingDuration = 0.3f;
        [Space]
        [SerializeField] private float _pointerEnterButtonScale = 1.5f;
        [SerializeField] private float _buttonScaleDuration = 0.25f;
        [SerializeField] private Color _buttonNormalColor;
        [SerializeField] private Color _buttonSelectedColor;

        public float ThumbnailChangingDuration => _thumbnailChangingDuration;
        public float LevelDescriptionVisibilityChangingDuration => _levelDescriptionVisibilityChangingDuration;
        public float PointerEnterButtonScale => _pointerEnterButtonScale;
        public float ButtonScaleDuration => _buttonScaleDuration;
        public Color ButtonNormalColor => _buttonNormalColor;
        public Color ButtonSelectedColor => _buttonSelectedColor;
    }
}
