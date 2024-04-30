using System;
using UnityEngine;
using UnityEngine.Localization;

namespace Scripts
{
    public enum ChallengeType
    {
        SolveTask,
        NoTips,
        CompletingTimeLessThan
    }

    [CreateAssetMenu(fileName = "Task Content", menuName = "Game Content/Task Content", order = 10)]
    public class TaskContent : ScriptableObject
    {
        [field: SerializeField]
        public LocalizedString Title { get; private set; }
        [field: SerializeField]
        public LocalizedString Description { get; private set; }
        [field: SerializeField, TextArea(3, 10)]
        public string StartCode { get; private set; }
        [field: SerializeField]
        public TaskTestData TestData { get; private set; }
        [field: SerializeField]
        public LocalizedString[] Tips { get; private set; }
        [field: SerializeField]
        public ChallengesData Challenges { get; private set; }
    }

    [Serializable]
    public class TaskTestData
    {
        [field: SerializeField, TextArea(10, 30)]
        public string TestCode { get; private set; }
        [field: SerializeField]
        public string TestMethodName { get; private set; }
        [field: SerializeField]
        public string PlayerCodePlaceholder { get; private set; }
        [field: SerializeField]
        public int PlayerCodeStartRowNumber { get; private set; }
    }
}
