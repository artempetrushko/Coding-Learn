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

        /*public void ChangeCallAvailability(bool isCallAvailable)
        {
            TargetPanelBehaviour.IsCallAvailable = isCallAvailable;
            PadMenuBehaviour.IsCallAvailable = isCallAvailable;
        }

        public void HideUI()
        {
            if (TargetPanelBehaviour.IsShown)
                TargetPanelBehaviour.HideTarget();
            TargetPanelBehaviour.IsCallAvailable = false;
            Minimap.SetActive(false);
        }

        public void ShowUI()
        {
            TargetPanelBehaviour.IsCallAvailable = true;
            Minimap.SetActive(true);
        }*/

        /*public IEnumerator MakeExitToMenuAvailable_COR()
        {
            yield return new WaitForSeconds(1.5f);
            isExitToMenuAvailable = true;
        }*/

        private void InitializeUiManager()
        {
            if (Instance == null)
                Instance = this;
            //ActionButtonBehaviour = ActionButton.GetComponent<ActionButtonBehaviour>();
        }

        private void Awake()
        {
            InitializeUiManager();
        }
    }
}
