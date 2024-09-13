using UnityEngine;

namespace GameLogic
{
    [CreateAssetMenu(fileName = "Level Content", menuName = "Game Content/Level Content", order = 10)]
    public class LevelContent : ScriptableObject
    {
        [SerializeField] private string _id;

        public string Id => _id;

        [field: SerializeField]
        public QuestConfig[] Quests { get; private set; }
        [field: SerializeField]
        public StoryContent EndingStoryPart { get; private set; }
        [field: SerializeField]
        public TrainingTheme[] HandbookAvailableThemes { get; private set; }
    }
}
