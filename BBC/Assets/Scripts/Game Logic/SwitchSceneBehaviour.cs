using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchSceneBehaviour : MonoBehaviour
{
    public Camera PreviousCamera;
    public Camera NextCamera;
    public GameObject DestinationTrigger;
    [Tooltip("������ �� ����-����� ���� ��������")]
    public bool IsMinimapShouldActive;
}
