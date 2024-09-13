using System;
using UnityEngine;
using UnityEngine.Localization;

namespace GameLogic
{
    [Serializable]
    public class ActionTimerTextsConfig
    {
        [SerializeField] private LocalizedString _actionAvailableText;
        [SerializeField] private LocalizedString _actionUnavailableText;
        [SerializeField] private LocalizedString _actionWaitingText;

        public LocalizedString ActionAvailableText => _actionAvailableText;
        public LocalizedString ActionUnavailableText => _actionUnavailableText;
        public LocalizedString ActionWaitingText => _actionWaitingText;     
    }
}
