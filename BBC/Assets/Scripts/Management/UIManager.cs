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

        [Header("Чёрный экран (контейнер)")]
        public GameObject BlackScreen;
        [Header("Загрузочный экран")]
        public GameObject LoadScreen;

        [Header("Скрипты UI-элементов")]
        [Tooltip("Скрипты UI-элементов для взаимодействия между собой")]
        public TaskPanel TaskPanelBehaviour;

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
