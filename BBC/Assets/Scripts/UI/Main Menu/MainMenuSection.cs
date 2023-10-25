using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class MainMenuSection : MonoBehaviour
    {
        [SerializeField]
        private GameObject buttonsContainer;
        [SerializeField]
        private Button mainMenuButtonPrefab;

        public void CreateButtons(List<MainMenuButtonData> buttonDatas)
        {
            for (var i = 0; i < buttonDatas.Count; i++)
            {
                var newButton = Instantiate(mainMenuButtonPrefab, buttonsContainer.transform);
                var currentIndex = i;
                newButton.GetComponentInChildren<TMP_Text>().text = buttonDatas[currentIndex].Text;
                newButton.onClick.AddListener(() => buttonDatas[currentIndex].buttonClicked.Invoke());
            }
        }
    }
}
