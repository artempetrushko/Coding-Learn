using System;
using Backend;
using Backend.EmailAuthentication;
using Unisave.Facades;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 * EmailAuthentication template - v0.9.1
 * -------------------------------------
 *
 * This script controls the register form and makes registration requests.
 *
 * Reference required UI elements and specify what scene to load
 * after registration.
 */

namespace Scripts.Unisave
{
    public class EmailRegisterForm : MonoBehaviour
    {
        public InputField emailField;
        public InputField passwordField;
        public InputField confirmPasswordField;
        public Button registerButton;
        public Text statusText;

        public InputField nicknameField;
        public Button chooseNicknameButton;
        public Text chooseStatus;
    
        void Start()
        {
            if (emailField == null)
                throw new ArgumentException(
                    $"Link the '{nameof(emailField)}' in the inspector."
                );
        
            if (passwordField == null)
                throw new ArgumentException(
                    $"Link the '{nameof(passwordField)}' in the inspector."
                );
        
            if (confirmPasswordField == null)
                throw new ArgumentException(
                    $"Link the '{nameof(confirmPasswordField)}' in the inspector."
                );
        
            if (registerButton == null)
                throw new ArgumentException(
                    $"Link the '{nameof(registerButton)}' in the inspector."
                );
        
            if (statusText == null)
                throw new ArgumentException(
                    $"Link the '{nameof(statusText)}' in the inspector."
                );
        
            registerButton.onClick.AddListener(OnRegisterClicked);
            chooseNicknameButton.onClick.AddListener(ChooseNickname);
            statusText.enabled = false;
            chooseStatus.enabled = false;
        }

        public async void OnRegisterClicked()
        {
            statusText.enabled = true;
            statusText.text = "Регистрация...";

            if (passwordField.text != confirmPasswordField.text)
            {
                statusText.text = "Пароли не совпадают!";
                return;
            }
        
            var response = await OnFacet<EmailRegisterFacet>
                .CallAsync<EmailRegisterResponse>(
                    nameof(EmailRegisterFacet.Register),
                    emailField.text,
                    passwordField.text
                );

            switch (response)
            {
                case EmailRegisterResponse.Ok:
                    statusText.enabled = false;             
                    StartCoroutine(gameObject.GetComponent<PlayerPanelBehaviour>().SwitchForms());
                    break;
            
                case EmailRegisterResponse.EmailTaken:
                    statusText.text = "Этот e-mail уже используется!";
                    break;
            
                case EmailRegisterResponse.InvalidEmail:
                    statusText.text = "Некорректный e-mail!";
                    break;
            
                case EmailRegisterResponse.WeakPassword:
                    statusText.text = "Пароль должен содержать как минимум 8 символов!";
                    break;
            
                default:
                    statusText.text = "Unknown response: " + response;
                    break;
            }
        }

        public async void ChooseNickname()
        {
            chooseStatus.enabled = true;
            chooseStatus.text = "Проверка уникальности...";
            var response = await OnFacet<EmailRegisterFacet>.CallAsync<bool>(
                nameof(EmailRegisterFacet.CheckNicknameUnique),
                emailField.text,
                passwordField.text,
                nicknameField.text);
            if (response)
            {
                chooseStatus.enabled = false;
                var player = await OnFacet<PlayerDataFacet>.CallAsync<Tuple<string, int>>(
                    nameof(PlayerDataFacet.GetPlayerData),
                    emailField.text);
                gameObject.GetComponent<PlayerPanelBehaviour>().PlayerName.text = player.Item1;
                gameObject.GetComponent<PlayerPanelBehaviour>().PlayerScore.text = player.Item2.ToString();
                StartCoroutine(gameObject.GetComponent<PlayerPanelBehaviour>().ShowPlayerInfo_PostRegistration());
            }
            else chooseStatus.text = "Игрок с таким никнеймом уже существует!";
        }
    }
}

