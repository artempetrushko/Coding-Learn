using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public enum PadMode
    {
        Normal,
        Development,
        HandbookMainThemes,
        HandbookSubThemes,
        HandbookProgrammingInfo
    }

    public class PadManager : MonoBehaviour
    {
        [HideInInspector] public PadMode PadMode;
    }
}
