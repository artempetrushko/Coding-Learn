using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerData : MonoBehaviour
{
    public enum Purpose
    {
        Task,
        Dialog,
        ScriptMoment,
        EnterToMiniScene,
        ChangeLevel,
        SaveGame,
        FinishLevel
    }

    [Tooltip("��� ���� ������� ������������")]
    public Purpose TriggerPurpose;
    [Tooltip("����� ��������")]
    public int TriggerNumber;
    [Tooltip("����� �������")]
    public int TaskNumber;
    [Tooltip("����� ��� ������ ��������������")]
    public string ActionButtonText;
    [Tooltip("������ ������, �� ������� ���������")]
    public int NextLevelIndex;

    public static readonly string MarkerAnimation = "RotateExclamationMark";
}
