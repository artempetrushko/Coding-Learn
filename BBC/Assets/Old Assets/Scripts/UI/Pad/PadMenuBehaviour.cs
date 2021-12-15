using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public enum PadMode
    {
        Normal,
        Development,
        HandbookMainThemes,
        HandbookSubThemes,
        HandbookProgrammingInfo
    }

    public class PadMenuBehaviour : MonoBehaviour
    {
        [Header("Планшет (главное меню)")]
        [Tooltip("Планшет")]
        public GameObject Pad;
        [Tooltip("Счетчик подсказок в меню")]
        public Text TipsMenuCounter;
        [Tooltip("Счетчик монет в меню")]
        public Text CoinsMenuCounter;
        [Tooltip("Кнопка перехода в режим разработки")]
        public Button ShowIDEButton;
        [Tooltip("Кнопка перехода в режим справочника")]
        public Button ShowHandbookButton;

        [HideInInspector] public List<TipsData> AvailableTipsCounts;
        [HideInInspector] public bool IsPadCalled;
        [HideInInspector] public bool IsCallAvailable;

        private UIManager uiManager;
        private GameManager gameManager;
        private PlayerBehaviour playerBehaviour;

        public void ChangeAvailability(bool isAvailable) => IsCallAvailable = isAvailable;

        public void UpdatePadData()
        {
            CoinsMenuCounter.text = gameManager.CoinsCount.ToString();
        }

        /*public void PlayPadMoveAnimation(string normalAnimation, string devAnimation)
        {
            var padMode = uiManager.PadMode;
            var padAnimator = Pad.GetComponent<Animator>();
            if (padMode == PadMode.Normal)
                padAnimator.Play(normalAnimation);
            else if (padMode == PadMode.Development)
                padAnimator.Play(devAnimation);
        }  */

        private IEnumerator CallPad()
        {
            if (!IsPadCalled)
            {
                playerBehaviour.FreezePlayer();
                Pad.GetComponent<Animator>().Play("ShowPad");
                IsPadCalled = !IsPadCalled;
            }
            else
            {
                if (uiManager.PadMode == PadMode.Normal)
                {
                    playerBehaviour.UnfreezePlayer();
                    Pad.GetComponent<Animator>().Play("HidePad");
                    IsPadCalled = !IsPadCalled;
                }
            }
            yield return new WaitForSeconds(0.667f);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P) && IsCallAvailable && gameManager.SceneIndex != 0)
                StartCoroutine(CallPad());
        }

        private void Start()
        {
            uiManager = UIManager.Instance;
            gameManager = GameManager.Instance;
            playerBehaviour = gameManager.Player.GetComponent<PlayerBehaviour>();
            uiManager.PadMode = PadMode.Normal;
            IsPadCalled = false;
            IsCallAvailable = true;
            ShowIDEButton.interactable = gameManager.SceneIndex == 0;
            AvailableTipsCounts = gameManager.AvailableTipsData;
            UpdatePadData();
        }
    }
}