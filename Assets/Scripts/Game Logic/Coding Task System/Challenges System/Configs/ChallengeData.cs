using System;
using UnityEngine;
using UnityEngine.Localization;

namespace GameLogic
{
    [Serializable]
    public class ChallengeData
    {
        [SerializeField] private string _id;
        [SerializeField] private LocalizedString _description;
        [SerializeField] private ChallengeCompletingChecker _checker;

        public string Id => _id;
        public LocalizedString Description => _description;
        public ChallengeCompletingChecker Checker => _checker;
    }
}
