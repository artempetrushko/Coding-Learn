using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    [CreateAssetMenu(fileName = "C# Keywords Data", menuName = "Game Data/Game/UI/Dev Environment/C# Keywords Data")]
    public class ProgrammingWordsHighlightConfig : ScriptableObject
    {
        [SerializeField] private Color _methodNameColor;
        [SerializeField] private Color _localVariablesColor;
        [SerializeField] private List<ProgrammingKeywordsData> _keywordColors = new();

        public Color MethodNameColor => _methodNameColor;
        public Color LocalVariablesColor => _localVariablesColor;
        public List<ProgrammingKeywordsData> KeywordColors => _keywordColors;
    }
}
