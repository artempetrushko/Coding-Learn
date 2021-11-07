using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
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

        [Tooltip("Для чего триггер предназначен")]
        public Purpose TriggerPurpose;
        [Tooltip("Текст для кнопки взаимодействия")]
        public string ActionButtonText;

        [Tooltip("Номер триггера")]
        public int ScriptMoment_TriggerNumber;

        [Tooltip("Номер задания")]
        public int Task_TaskNumber;

        [Tooltip("Индекс уровня, на который переходим")]
        public int ChangeLevel_NextLevelIndex;

        [Tooltip("Триггер, к которому переместится игрок")]
        public GameObject EnterToMiniScene_DestinationTrigger;
        [Tooltip("Должна ли мини-карта быть активной")]
        public bool EnterToMiniScene_IsMinimapShouldActive;
    }
}
