using UnityEngine;

namespace GameLogic
{
    [CreateAssetMenu(fileName = "C# Keywords Config", menuName = "Game Configs/Dev Environment/C# Keywords Config")]
    public class ProgrammingWordsHighlightConfig : ScriptableObject
    {
        [SerializeField] private Color _methodNameColor;
        [SerializeField] private Color _localVariablesColor;
        [SerializeField] private ProgrammingKeywordsConfig[] _keywordColors;

        public Color MethodNameColor => _methodNameColor;
        public Color LocalVariablesColor => _localVariablesColor;
        public ProgrammingKeywordsConfig[] KeywordColors => _keywordColors;
    }
}
