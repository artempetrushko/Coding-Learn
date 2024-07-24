using UnityEngine;
using UnityEngine.Localization;

namespace Scripts
{
    public abstract class TrainingContent : ScriptableObject
    {
        [field: SerializeField]
        public LocalizedString Title { get; private set; }
    }
}
