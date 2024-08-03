using UnityEngine;

namespace Scripts
{
    [CreateAssetMenu(fileName = "Dev Environment Section Config", menuName = "Game Data/Dev Environment/Dev Environment Section Config")]
    public class DevEnvironmentSectionConfig : ScriptableObject
    {
        [SerializeField] private Color _successColor = Color.green;
        [SerializeField] private Color _failureColor = Color.red;

        [SerializeField] private Color _progressBarNormalColor = Color.blue;    
        [SerializeField] private float _progressBarFillingDuration = 3f;

        [SerializeField] private float _taskCompletingIndicatorEndAlpha = 0.5f;
        [SerializeField] private float _taskCompletingIndicatorAlphaChangingDuration = 0.7f;

        public Color ProgressBarNormalColor => _progressBarNormalColor;
        public Color SuccessColor => _successColor;
        public Color FailureColor => _failureColor;
        public float ProgressBarFillingDuration => _progressBarFillingDuration;
        public float TaskCompletingIndicatorEndAlpha => _taskCompletingIndicatorEndAlpha;
        public float TaskCompletingIndicatorAlphaChangingDuration => _taskCompletingIndicatorAlphaChangingDuration;
    }
}
