using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    [CreateAssetMenu(fileName = "C# Keywords Data", menuName = "Game Data/Game/UI/Dev Environment/C# Keywords Data")]
    public class ProgrammingWordsHighlightData : ScriptableObject
    {
        [SerializeField]
        private Color methodNameColor;
        [SerializeField]
        private Color localVariablesColor;
        [SerializeField]
        private List<ProgrammingKeywordsData> keywordColors = new List<ProgrammingKeywordsData>();

        public Color MethodNameColor => methodNameColor;
        public Color LocalVariablesColor => localVariablesColor;
        public List<ProgrammingKeywordsData> KeywordColors => keywordColors;
    }
}
