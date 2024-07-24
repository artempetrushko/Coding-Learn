using System;
using UnityEngine;

namespace Scripts
{
    [Serializable]
    public class TaskTestData
    {
        [field: SerializeField, TextArea(10, 30)]
        public string TestCode { get; private set; }
        [field: SerializeField]
        public TaskTestSettings TestSettings { get; private set; }
    }
}
