using UnityEngine;
using UnityEngine.Localization;

namespace Scripts
{
    [CreateAssetMenu(fileName = "Tips Section Button Labels Config", menuName = "Game Data/Tips/Tips Section Button Labels Config")]
    public class TipSectionLabelsData : ScriptableObject
    {
        [SerializeField] private LocalizedString _actionAvailableText;
        [SerializeField] private LocalizedString _actionUnavailableText;
        [SerializeField] private LocalizedString _actionWaitingText;

        public LocalizedString ActionAvailableText => _actionAvailableText;
        public LocalizedString ActionUnavailableText => _actionUnavailableText;
        public LocalizedString ActionWaitingText => _actionWaitingText;     
    }
}
