using System;
using UnityEngine;
using UnityEngine.Localization;

namespace Scripts
{
    [Serializable]
    public class TipSectionLabelsData
    {
        [field: SerializeField]
        public LocalizedString ActionAvailableLabel { get; private set; }
        [field: SerializeField]
        public LocalizedString ActionUnavailableLabel { get; private set; }
        [field: SerializeField]
        public LocalizedString ActionWaitingLabel { get; private set; }        
    }
}
