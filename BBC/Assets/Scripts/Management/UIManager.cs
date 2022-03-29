using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance = null;

        [Header("���������")]
        public Canvas Canvas;

        [Header("������")]
        [Tooltip("������ ���������� �������� (��������� �������, ����� ����� � �.�.)")]
        public Button ActionButton;

        [Header("׸���� ����� (���������)")]
        public GameObject BlackScreen;
        [Header("����������� �����")]
        public GameObject LoadScreen;

        [Header("������� UI-���������")]
        [Tooltip("������� UI-��������� ��� �������������� ����� �����")]
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
