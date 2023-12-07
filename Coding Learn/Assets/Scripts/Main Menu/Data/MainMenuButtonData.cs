using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

namespace Scripts
{
    [Serializable]
    public class MainMenuButtonData
    {
        public LocalizedString LocalizedString;
        public UnityEvent onButtonPressed;
    }
}
