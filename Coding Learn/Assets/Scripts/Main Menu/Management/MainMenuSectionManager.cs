using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public abstract class MainMenuSectionManager : MonoBehaviour
    {
        public abstract IEnumerator ShowSectionView_COR();
        public abstract IEnumerator HideSectionView_COR();
    }
}
