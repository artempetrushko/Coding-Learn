using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    [Serializable]
    public class ProgrammingKeywordsData
    {
        [SerializeField]
        private Color color;
        [SerializeField]
        private List<string> keywords = new List<string>();

        public Color Color => color;
        public List<string> Keywords => keywords;       
    }
}
