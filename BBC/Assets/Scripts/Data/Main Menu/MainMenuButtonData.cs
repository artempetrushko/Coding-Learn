using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts
{
    [Serializable]
    public class MainMenuButtonData
    {
        public string Text;
        public UnityEvent buttonClicked;
    }
}
