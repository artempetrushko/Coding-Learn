using System;
using UnityEngine;
using UnityEngine.Timeline;

namespace Scripts
{
    [CreateAssetMenu(fileName = "Level Content", menuName = "Game Content/Level Content", order = 10)]
    public class LevelContent : ScriptableObject
    {
        [field: SerializeField]
        public QuestContent[] Quests { get; private set; }
        [field: SerializeField]
        public TimelineAsset EndingCutscene { get; private set; }
    }

    [Serializable]
    public class QuestContent
    {
        [field: SerializeField]
        public StoryContent Story { get; private set; }
        [field: SerializeField]
        public TrainingTheme TrainingTheme { get; private set; }
        [field: SerializeField]
        public TaskContent Task { get; private set; }
    }
}
