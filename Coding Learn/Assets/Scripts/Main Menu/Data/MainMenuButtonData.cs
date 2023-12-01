using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Scripts
{
    [Serializable]
    public class MainMenuButtonData
    {
        public LocalizedString LocalizedString;
        public string LocalizedTextReference;
        public MainMenuSectionManager LinkedSection;
    }
}
