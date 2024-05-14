using UnityEngine;

namespace Scripts
{
    [CreateAssetMenu(fileName = "Task Test Settings", menuName = "Game Content/Task Test Settings", order = 10)]
    public class TaskTestSettings : ScriptableObject
    {
        [field: SerializeField]
        public string StartMethodName { get; private set; }
        [field: SerializeField]
        public string PlayerCodePlaceholder { get; private set; }
    }
}
