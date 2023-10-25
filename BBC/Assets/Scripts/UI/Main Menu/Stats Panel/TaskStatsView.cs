using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Scripts
{
    public class TaskStatsView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text taskTitleText;
        [SerializeField]
        private TMP_Text starsCounterText;

        public void SetInfo(string taskTitle, string starsCountFormattedText)
        {
            taskTitleText.text = taskTitle;
            starsCounterText.text = starsCountFormattedText;
        }
    }
}
