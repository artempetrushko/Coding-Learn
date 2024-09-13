using System;
using UnityEngine;

namespace GameLogic
{
    [Serializable]
    public class TaskTestData
    {
        [SerializeField] private string _testMethodName;
        [SerializeField] private string _playerCodePlaceHolder;

        public string TestMethodName => _testMethodName; 
        public string PlayerCodePlaceholder => _playerCodePlaceHolder;


        [field: SerializeField, TextArea(10, 30)]
        public string TestCode { get; private set; }
    }
}
