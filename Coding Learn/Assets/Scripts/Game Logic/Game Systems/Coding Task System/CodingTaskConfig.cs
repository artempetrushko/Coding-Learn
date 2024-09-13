using UnityEngine;
using UnityEngine.Localization;

namespace GameLogic
{
    [CreateAssetMenu(fileName = "Task Content", menuName = "Game Content/Task Content", order = 10)]
    public class CodingTaskConfig : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private LocalizedString _title;
        [SerializeField] private LocalizedString _description;
        [SerializeField] private ChallengesConfig _challengesConfig;

        public string Id => _id;
        public LocalizedString Title => _title;
        public LocalizedString Description => _description;

        [field: SerializeField, TextArea(3, 10)]
        public string StartCode { get; private set; }
        [field: SerializeField]
        public TaskTestData TestData { get; private set; }
        [field: SerializeField]
        public LocalizedString[] Tips { get; private set; }

        public ChallengesConfig ChallengesConfig => _challengesConfig;
    }
}
