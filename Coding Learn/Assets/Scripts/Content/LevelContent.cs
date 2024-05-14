using UnityEngine;

namespace Scripts
{
    [CreateAssetMenu(fileName = "Level Content", menuName = "Game Content/Level Content", order = 10)]
    public class LevelContent : ScriptableObject
    {
        [field: SerializeField]
        public QuestContent[] Quests { get; private set; }
        [field: SerializeField]
        public StoryContent EndingStoryPart { get; private set; }
        [field: SerializeField]
        public TrainingTheme[] HandbookAvailableThemes { get; private set; }
    }
}
