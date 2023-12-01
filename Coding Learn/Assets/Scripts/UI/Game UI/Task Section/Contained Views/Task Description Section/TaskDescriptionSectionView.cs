using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class TaskDescriptionSectionView : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text taskTitleText;
        [SerializeField]
        private TMP_Text taskDescriptionText;
        [SerializeField]
        private Scrollbar scrollbar;

        public void SetContent(string taskTitle, string taskDesription)
        {
            taskTitleText.text = taskTitle;
            taskDescriptionText.text = taskDesription;
            scrollbar.value = 1;
        }
    }
}
