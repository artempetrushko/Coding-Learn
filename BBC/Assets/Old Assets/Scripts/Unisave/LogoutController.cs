using System;
using Backend.EmailAuthentication;
using Unisave.Facades;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Unisave
{
    public class LogoutController : MonoBehaviour
    {
        public Button logoutButton;

        private void Start()
        {
            if (logoutButton == null)
                throw new ArgumentNullException(
                    nameof(logoutButton),
                    nameof(logoutButton) + " field has not been linked."
                );
            
            logoutButton.onClick.AddListener(LogoutButtonClicked);
        }

        private async void LogoutButtonClicked()
        {
            var wasLoggedIn = await OnFacet<EmailLoginFacet>.CallAsync<bool>(
                nameof(EmailLoginFacet.Logout)
            );
            
            LogoutSucceeded(wasLoggedIn);
        }

        private void LogoutSucceeded(bool wasLoggedIn)
        {
            // implement your own logic here

            if (wasLoggedIn)
            {
                Debug.Log("Logout successful!");
                gameObject.GetComponent<PlayerPanelBehaviour>().ReturnToLoginAndRegistrationPanel_PlayerInfoPanel();
                gameObject.GetComponent<MenuScript>().ChangeMainMenuButtonsAvailability(false);
            }
            else
                Debug.Log("There was no player to logout.");
        }
    }
}