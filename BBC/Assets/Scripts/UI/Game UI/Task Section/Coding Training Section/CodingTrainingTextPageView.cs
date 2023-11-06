using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class CodingTrainingTextPageView : MonoBehaviour
    {
        [SerializeField]
        protected TMP_Text trainingText;
        [SerializeField]
        protected Scrollbar trainingTextSlider;

        public void SetContent(string trainingContent)
        {
            trainingText.text = trainingContent;
            trainingTextSlider.value = 1;
        }
    }
}
