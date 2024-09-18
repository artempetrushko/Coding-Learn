using System;
using UnityEngine;

namespace GameLogic
{
    [Serializable]
    public class QuestConfig
    {
        [SerializeField] private StoryContent _story;
        [SerializeField] private TrainingSubTheme _trainingSubTheme;
        [SerializeField] private CodingTaskConfig _task;

        public StoryContent Story => _story;
        public TrainingSubTheme TrainingSubTheme => _trainingSubTheme;
        public CodingTaskConfig Task => _task;
    }
}
