using System;
using UnityEngine;

namespace Scripts
{
    [Serializable]
    public class QuestContent
    {
        [field: SerializeField]
        public StoryContent Story { get; private set; }
        [field: SerializeField]
        public TrainingSubTheme TrainingSubTheme { get; private set; }
        [field: SerializeField]
        public TaskContent Task { get; private set; }
    }
}
