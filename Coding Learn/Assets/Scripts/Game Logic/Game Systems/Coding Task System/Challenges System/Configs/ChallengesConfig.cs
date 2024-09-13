using UnityEngine;

namespace GameLogic
{
    [CreateAssetMenu(fileName = "Challenges Config", menuName = "Game Content/Challenges Config", order = 10)]
    public class ChallengesConfig : ScriptableObject
    {
        [SerializeField] private ChallengeData[] _challenges;

        public ChallengeData[] Challenges => _challenges;
    }
}
