using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    [Serializable]
    public class ProgrammingKeywordsData
    {
        [SerializeField] private Color _color;
        [SerializeField] private List<string> _keywords = new();

        public Color Color => _color;
        public List<string> Keywords => _keywords;       
    }
}
