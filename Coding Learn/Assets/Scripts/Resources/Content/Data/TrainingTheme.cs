using System;
using UnityEngine;
using UnityEngine.Localization;

namespace Scripts
{
    [Serializable]
    public abstract class TrainingContent : ScriptableObject
    {
        [field: SerializeField]
        public LocalizedString Title { get; private set; }
    }

    [CreateAssetMenu(fileName = "Training Theme", menuName = "Game Content/Training/Training Theme", order = 10)]
    public class TrainingTheme : TrainingContent
    {
        [field: SerializeField]
        public TrainingSubTheme[] SubThemes { get; private set; }
    }
}
