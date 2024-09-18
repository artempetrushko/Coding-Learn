using UnityEngine;

namespace GameLogic
{
    [CreateAssetMenu(fileName = "Level Content", menuName = "Game Configs/Level Configs/Level Content")]
    public class LevelContent : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private QuestConfig[] _quests;
        [SerializeField] private StoryContent _endingStoryPart;
        [SerializeField] private TrainingTheme[] _handbookAvailableThemes;

        public string Id => _id;
        public QuestConfig[] Quests { get; private set; }
        public StoryContent EndingStoryPart { get; private set; }
        public TrainingTheme[] HandbookAvailableThemes { get; private set; }
    }
}
