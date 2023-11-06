using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class ErrorsSectionView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text errorsText;
        [SerializeField]
        private Scrollbar scrollbar;

        public void SetContent(string errorsMessage)
        {
            errorsText.text = errorsMessage;
            scrollbar.value = 1;
        }
    }
}
