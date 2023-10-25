using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scripts
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField]
        private LoadingBar loadingBar;

        public IEnumerator Show()
        {
            GetComponent<Animator>().Play("AppearLoadScreen");
            yield return new WaitForSeconds(0.75f);
            loadingBar.gameObject.SetActive(true);
        }

        public void SetBackground(Sprite backgroundImage)
        {
            GetComponent<Image>().sprite = backgroundImage;
        }

        public void SetLoadingBarAppearance(float loadingProgress, string loadingText)
        {
            loadingBar.InnerArea.fillAmount = loadingProgress;
            loadingBar.LoadingText.text = loadingText;
        }
    }
}
