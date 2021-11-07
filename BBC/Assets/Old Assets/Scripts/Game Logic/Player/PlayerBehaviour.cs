using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Scripts
{
    public class PlayerBehaviour : MonoBehaviour
    {
        [Header("Текущие значения скорости движения и поворота")]
        public float currentMoveSpeed;
        public float currentRotateSpeed;
        [Header("Начальные значения скорости движения и поворота")]
        public float moveSpeed = 6f;
        public float rotateSpeed = 6f;
        [Header("Камера игрока")]
        public GameObject PlayerCamera;

        private float playerCameraSpeedX;
        private float playerCameraSpeedY;

        public void FreezePlayer()
        {
            currentMoveSpeed = 0f;
            currentRotateSpeed = 0f;
            PlayerCamera.GetComponentInChildren<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = 0f;
            PlayerCamera.GetComponentInChildren<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = 0f;
        }

        public void UnfreezePlayer()
        {
            currentMoveSpeed = moveSpeed;
            currentRotateSpeed = rotateSpeed;
            PlayerCamera.GetComponentInChildren<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = playerCameraSpeedX;
            PlayerCamera.GetComponentInChildren<CinemachineFreeLook>().m_YAxis.m_MaxSpeed = playerCameraSpeedY;
        }

        private void FixedUpdate()
        {
            var deltaX = Input.GetAxis("Horizontal");
            var deltaZ = Input.GetAxis("Vertical");
            transform.Rotate(0f, deltaX * currentRotateSpeed, 0f);
            if (Input.GetKey(KeyCode.LeftShift))
                transform.Translate(new Vector3(0f, 0f, -deltaZ * currentMoveSpeed * 1.5f * Time.deltaTime));
            else transform.Translate(new Vector3(0f, 0f, -deltaZ * currentMoveSpeed * Time.deltaTime));
        }

        private void Awake()
        {
            currentMoveSpeed = moveSpeed;
            currentRotateSpeed = rotateSpeed;
            playerCameraSpeedX = PlayerCamera.GetComponentInChildren<CinemachineFreeLook>().m_XAxis.m_MaxSpeed;
            playerCameraSpeedY = PlayerCamera.GetComponentInChildren<CinemachineFreeLook>().m_YAxis.m_MaxSpeed;
        }
    }
}
