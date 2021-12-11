using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts
{
    public class TrainingPanelBehaviour : MonoBehaviour
    {
        [Header("Панель обучения")]
        public GameObject TrainingPanel;
        public GameObject TrainingPanelBackground;

        [Header("Элементы UI для переключения состояния")]
        [SerializeField] private PadDevelopmentBehaviour Pad_Dev;

        private GameManager gameManager;
        private UIManager uiManager;

        public void ShowFirstTip(GameObject tip)
        {
            TrainingPanel.SetActive(true);
            TrainingPanelBackground.SetActive(true);
            tip.SetActive(true);
            tip.GetComponent<TipBehaviour>().ShowTip();
        }

        public void CloseTraining()
        {
            TrainingPanel.SetActive(false);
            TrainingPanelBackground.SetActive(false);
        }

        private void Start()
        {
            gameManager = GameManager.Instance;
            uiManager = UIManager.Instance;
        }
    }
}
