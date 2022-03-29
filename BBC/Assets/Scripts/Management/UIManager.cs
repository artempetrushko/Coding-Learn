using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance = null;

        [Header("Интерфейс")]
        public Canvas Canvas;

        [Header("Кнопки")]
        [Tooltip("Кнопка выполнения действия (активация задания, смена сцены и т.д.)")]
        public Button ActionButton;

        [Header("Чёрный экран (контейнер)")]
        public GameObject BlackScreen;
        [Header("Загрузочный экран")]
        public GameObject LoadScreen;

        [Header("Скрипты UI-элементов")]
        [Tooltip("Скрипты UI-элементов для взаимодействия между собой")]
        public PadMenuBehaviour PadMenuBehaviour;
        public TaskPanelBehaviour TaskPanelBehaviour;
        public TrainingPanelBehaviour TrainingPanelBehaviour;

        [HideInInspector] public InventoryBehaviour InventoryBehaviour;
        [HideInInspector] public ActionButtonBehaviour ActionButtonBehaviour;
        [HideInInspector] public PadMode PadMode;

        private void InitializeUiManager()
        {
            if (Instance == null)
                Instance = this;
        }

        private void Awake()
        {
            InitializeUiManager();
        }
    }
}
