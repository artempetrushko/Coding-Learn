using System;
using UnityEngine;

namespace GameLogic
{
	[Serializable]
    public class ProgrammingKeywordsConfig
    {
        [SerializeField] private Color _color;
        [SerializeField] private string[] _keywords;

        public Color Color => _color;
        public string[] Keywords => _keywords;       
    }
}
