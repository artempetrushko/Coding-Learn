using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class PlayerPanelBehaviour : MonoBehaviour
    {
        public GameObject PlayerPanel;
        public GameObject LoginAndRegistrationPanel;
        public GameObject ChooseNicknamePanel;
        public GameObject LoginForm;
        public GameObject RegistrationForm;
        public GameObject PlayerInfoPanel;
        public Text PlayerName;
        public Text PlayerScore;

        public void ShowLoginForm() => ShowForm(LoginForm);

        public void ShowRegistrationForm() => ShowForm(RegistrationForm);

        public void ReturnToLoginAndRegistrationPanel_LoginForm() => ReturnToLoginAndRegistrationPanel(LoginForm);

        public void ReturnToLoginAndRegistrationPanel_RegistrationForm() => ReturnToLoginAndRegistrationPanel(RegistrationForm);

        public void ShowPlayerInfo()
        {
            LoginForm.GetComponent<Animator>().Play("MoveForm_Right");
            PlayerInfoPanel.GetComponent<Animator>().Play("MovePlayerInfoPanel_Left");
            gameObject.GetComponent<MenuScript>().ChangeMainMenuButtonsAvailability(true);
        }

        public IEnumerator ShowPlayerInfo_PostRegistration()
        {
            gameObject.GetComponent<MenuScript>().IsPlayerLoggedIn = true;
            ChooseNicknamePanel.GetComponent<Animator>().Play("MoveChooseNicknamePanel_Right");
            PlayerInfoPanel.GetComponent<Animator>().Play("MovePlayerInfoPanel_Left");
            yield return new WaitForSeconds(0.583f);
            gameObject.GetComponent<MenuScript>().ChangeMainMenuButtonsAvailability(true);
            ChooseNicknamePanel.GetComponentInChildren<InputField>().text = "";
        }

        public void ShowPlayerInfo_PlayerAlreadyLoggedIn(string nickname, int totalScore)
        {
            PlayerName.text = nickname;
            PlayerScore.text = totalScore.ToString();
            LoginAndRegistrationPanel.GetComponent<Animator>().Play("MoveLoginAndRegistrationPanel_Right");
            PlayerInfoPanel.GetComponent<Animator>().Play("MovePlayerInfoPanel_Left");
            gameObject.GetComponent<MenuScript>().ChangeMainMenuButtonsAvailability(true);
        }

        public IEnumerator SwitchForms()
        {
            foreach (var button in RegistrationForm.GetComponentsInChildren<Button>())
                button.interactable = false;
            yield return new WaitForSeconds(1f);
            ChooseNicknamePanel.GetComponent<Animator>().Play("MoveChooseNicknamePanel_Left");
            RegistrationForm.GetComponent<Animator>().Play("MoveForm_Right");
            yield return new WaitForSeconds(0.583f);
            foreach (var button in RegistrationForm.GetComponentsInChildren<Button>())
                button.interactable = true;
        }

        public void ReturnToLoginAndRegistrationPanel_PlayerInfoPanel()
        {
            LoginAndRegistrationPanel.GetComponent<Animator>().Play("MoveLoginAndRegistrationPanel_Left");
            PlayerInfoPanel.GetComponent<Animator>().Play("MovePlayerInfoPanel_Right");
        }

        private void ShowForm(GameObject form)
        {
            foreach (var field in form.GetComponentsInChildren<InputField>())
                field.text = "";
            LoginAndRegistrationPanel.GetComponent<Animator>().Play("MoveLoginAndRegistrationPanel_Right");
            form.GetComponent<Animator>().Play("MoveForm_Left");
        }

        private void ReturnToLoginAndRegistrationPanel(GameObject form)
        {
            form.GetComponent<Animator>().Play("MoveForm_Right");
            LoginAndRegistrationPanel.GetComponent<Animator>().Play("MoveLoginAndRegistrationPanel_Left");
        }
    }
}