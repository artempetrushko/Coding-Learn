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

        [Header("׸���� ����� (���������)")]
        public GameObject BlackScreen;
        [Header("����������� �����")]
        public GameObject LoadScreen;

        [Header("������� UI-���������")]
        [Tooltip("������� UI-��������� ��� �������������� ����� �����")]
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
