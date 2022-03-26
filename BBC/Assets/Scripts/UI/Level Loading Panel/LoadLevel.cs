using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scripts
{
    public class LoadLevel : MonoBehaviour
    {
        public GameObject LoadScreen;

        [SerializeField] private Image LoadBar;
        [SerializeField] private Text LoadBarText;

        private GameManager gameManager;

        public void LoadLevelAsync() => StartCoroutine(LoadLevelAsync_COR(gameManager.SceneIndex));

        public IEnumerator LoadLevelAsync_COR(int sceneIndex)
        {
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

        private void Start()
        {
            gameManager = GameManager.Instance;
        }
    }
}
