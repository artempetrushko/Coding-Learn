using GameLogic;
using UnityEngine;

namespace GameLogic
{
    [CreateAssetMenu(fileName = "Dev Environment Section Config", menuName = "Game Configs/Dev Environment/Dev Environment Section Config")]
    public class DevEnvironmentConfig : ScriptableObject
    {
        [SerializeField] private ProgrammingWordsHighlightConfig _programmingWordsHighlightConfig;
        [SerializeField] private string _testFailureMessage;
        [SerializeField] private Color _successColor = Color.green;
        [SerializeField] private Color _failureColor = Color.red;
        [Space]
        [SerializeField] private Color _progressBarNormalColor = Color.blue;    

        public ProgrammingWordsHighlightConfig ProgrammingWordsHighlightConfig => _programmingWordsHighlightConfig;
        public string TestFailureMessage => _testFailureMessage;
        public Color ProgressBarNormalColor => _progressBarNormalColor;
        public Color SuccessColor => _successColor;
        public Color FailureColor => _failureColor;
    }
}
