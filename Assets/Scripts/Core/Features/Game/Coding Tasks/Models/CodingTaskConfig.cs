using GameLogic;
using UnityEngine;
using UnityEngine.Localization;

namespace Core.Features.Game.CodingTraining.Models
{
    [CreateAssetMenu(fileName = "Coding Task", menuName = "Game Configs/Quests/Coding Task")]
    public class CodingTaskConfig : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private LocalizedString _title;
        [SerializeField] private LocalizedString _description;
        [TextArea(3, 10)]
        [SerializeField] private string _startCode;
        [TextArea(3, 10)]
        [SerializeField] private string _testCode;
        [SerializeField] private string _testMethodName;
        [SerializeField] private string _playerCodePlaceHolder;
        [SerializeField] private LocalizedString[] _tips;

        public string Id => _id;
        public LocalizedString Title => _title;
        public LocalizedString Description => _description;
        public string StartCode => _startCode;
        public string TestCode => _testCode;
        public string TestMethodName => _testMethodName;
        public string PlayerCodePlaceholder => _playerCodePlaceHolder;
        public LocalizedString[] Tips => _tips;
    }
}
