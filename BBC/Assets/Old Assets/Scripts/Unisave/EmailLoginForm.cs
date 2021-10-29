using System;
using Backend;
using Backend.EmailAuthentication;
using Unisave.Facades;
using UnityEngine;
using UnityEngine.UI;

/*
 * EmailAuthentication template - v0.9.1
 * -------------------------------------
 *
 * This script controls the login form and makes login requests.
 *
 * Reference required UI elements and specify what scene to load after login.
 */

namespace Scripts.Unisave
{
    public class EmailLoginForm : MonoBehaviour
    {
        public InputField emailField;
        public InputField passwordField;
        public Button loginButton;
        public Text statusText;

        private PlayerPanelBehaviour playerPanelBehaviour;

        void Start()
        {
            playerPanelBehaviour = gameObject.GetComponent<PlayerPanelBehaviour>();

            if (emailField == null)
                throw new ArgumentException(
                    $"Link the '{nameof(emailField)}' in the inspector."
                );
        
            if (passwordField == null)
                throw new ArgumentException(
                    $"Link the '{nameof(passwordField)}' in the inspector."
                );
        
            if (loginButton == null)
                throw new ArgumentException(
                    $"Link the '{nameof(loginButton)}' in the inspector."
                );
        
            if (statusText == null)
                throw new ArgumentException(
                    $"Link the '{nameof(statusText)}' in the inspector."
                );
        
            loginButton.onClick.AddListener(OnLoginClicked);

            statusText.enabled = false;
        }

        public async void OnLoginClicked()
        {
            statusText.enabled = true;
            statusText.text = "Выполняется вход...";
        
            var response = await OnFacet<EmailLoginFacet>.CallAsync<bool>(
                nameof(EmailLoginFacet.Login),
                emailField.text,
                passwordField.text
            );

            if (response)
            {
                statusText.enabled = false;
                var player = await OnFacet<PlayerDataFacet>.CallAsync<Tuple<string, int>>(
                    nameof(PlayerDataFacet.GetPlayerData),
                    emailField.text);
                playerPanelBehaviour.PlayerName.text = player.Item1;
                playerPanelBehaviour.PlayerScore.text = player.Item2.ToString();
                gameObject.GetComponent<MenuScript>().IsPlayerLoggedIn = true;
                gameObject.GetComponent<PlayerPanelBehaviour>().ShowPlayerInfo();
            }
            else
            {
                statusText.text = "Неверный логин или пароль";
            }
        }
    }
}

