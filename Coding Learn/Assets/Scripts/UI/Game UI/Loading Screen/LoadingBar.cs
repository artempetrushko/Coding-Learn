using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class LoadingBar : MonoBehaviour
    {
        [SerializeField]
        private Image innerArea;
        [SerializeField]
        private TMP_Text loadingText;

        public Image InnerArea => innerArea;
        public TMP_Text LoadingText => loadingText;
    }
}
