using UnityEngine;

namespace GameLogic
{
    [CreateAssetMenu(fileName = "Challenges Config", menuName = "Game Configs/Challenges/Challenges Config")]
    public class ChallengesConfig : ScriptableObject
    {
        [SerializeField] private ChallengeData[] _challenges;

        public ChallengeData[] Challenges => _challenges;
    }
}
