using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scripts
{
    public class LoadLevel : MonoBehaviour
    {
        [SerializeField] private GameObject LoadScreen;
        [SerializeField] private Image LoadBar;
        [SerializeField] private Text LoadBarText;

        public void LoadLevelAsync()
        {
            var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            if (currentSceneIndex == SceneManager.sceneCountInBuildSettings - 1)
                StartCoroutine(LoadLevelAsync_COR(0));
            else StartCoroutine(LoadLevelAsync_COR(currentSceneIndex + 1));
        }

        public IEnumerator LoadLevelAsync_COR(int sceneIndex)
        {
            if (sceneIndex != 0)
                LoadScreen.GetComponent<Image>().sprite = Resources.Load<Sprite>("Load Screens/LoadScreen_Level" + sceneIndex);
            LoadScreen.GetComponent<Animator>().Play("AppearLoadScreen");
            yield return new WaitForSeconds(0.75f);
            LoadScreen.transform.GetChild(0).gameObject.SetActive(true);
            var operation = SceneManager.LoadSceneAsync(sceneIndex);
            while (!operation.isDone)
            {
                LoadBar.fillAmount = operation.progress;
                LoadBarText.text = "Загрузка... " + (Mathf.Round(operation.progress * 100)) + "%";
                yield return null;
            }
        }
    }
}
