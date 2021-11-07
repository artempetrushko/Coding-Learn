using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class TargetPanelBehaviour : MonoBehaviour
    {
        [Header("Панель текущей цели")]
        [Tooltip("Панель текущей цели")]
        public GameObject TargetPanel;
        [Tooltip("Фон панели текущей цели")]
        public GameObject TargetPanelBackground;
        [Tooltip("Надпись Цель")]
        public Text TargetLabel;
        [Tooltip("Текстовое описание цели")]
        public Text TargetText;

        [HideInInspector] public string TargetLabelText;
        [HideInInspector] public bool IsShown;
        [HideInInspector] public bool IsCallAvailable;

        private int timer;

        public void ChangeAvailability(bool isAvailable) => IsCallAvailable = isAvailable;

        public void ShowTarget() => StartCoroutine(ShowTarget_COR());

        public void ChangeTarget(string newTargetText)
        {
            GameManager.Instance.Target = newTargetText;
            StartCoroutine(ShowTarget_COR());
        }

        public void HideTarget()
        {
            for (var i = 0; i < 3; i++)
            {
                var background = TargetPanelBackground.transform.GetChild(i).gameObject;
                background.GetComponent<Image>().fillAmount = 0f;
                background.SetActive(false);
            }
            TargetLabel.text = "";
            TargetLabel.gameObject.SetActive(false);
            TargetText.text = "";
            TargetText.gameObject.SetActive(false);
            IsShown = false;
        }

        private IEnumerator ShowTarget_COR()
        {
            IsShown = true;
            timer = 5;
            for (var i = 0; i < 3; i++)
            {
                var background = TargetPanelBackground.transform.GetChild(i);
                background.gameObject.SetActive(true);
                background.GetComponent<Animator>().Play("DrawBackground");
            }
            yield return new WaitForSeconds(0.3f);
            TargetLabel.gameObject.SetActive(true);
            for (var i = 0; i < TargetLabelText.Length; i++)
            {
                TargetLabel.text += TargetLabelText[i];
                yield return new WaitForSeconds(0.01f);
            }
            TargetText.gameObject.SetActive(true);
            for (var i = 0; i < GameManager.Instance.Target.Length; i++)
            {
                TargetText.text += GameManager.Instance.Target[i];
                yield return new WaitForSeconds(0.01f);
            }
            StartCoroutine(HideTargetAfterTimer_COR());
        }

        private IEnumerator HideTargetAfterTimer_COR()
        {
            while (timer > 0)
            {
                yield return new WaitForSeconds(1.0f);
                timer--;
            }
            HideTarget();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftAlt) && IsCallAvailable)
            {
                if (timer == 0)
                    StartCoroutine(ShowTarget_COR());
                else timer = 5;
            }
        }

        private void Start()
        {
            TargetLabelText = TargetLabel.text;
            timer = 0;
            HideTarget();
            IsCallAvailable = false;
        }
    }
}