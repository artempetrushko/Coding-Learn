using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts
{
    public class SceneLoadingManager : MonoBehaviour
    {
        [SerializeField] 
        private LoadingScreenView loadingScreenView;

        public IEnumerator LoadNextSceneAsync_COR(int sceneIndex)
        {
            if (sceneIndex > 0)
            {
                loadingScreenView.gameObject.SetActive(true);
                loadingScreenView.SetBackground(GameContentManager.GetLoadingScreenBackground(sceneIndex + 1));
                yield return StartCoroutine(loadingScreenView.Show_COR());

                var operation = SceneManager.LoadSceneAsync(sceneIndex);
                while (!operation.isDone)
                {
                    loadingScreenView.SetLoadingBarAppearance(operation.progress, string.Format(@"Загрузка... {0}%", Mathf.Round(operation.progress * 100)));
                    yield return null;
                }
            }
            else
            {
                LoadMainMenu();
            }
        }

        public void LoadMainMenu() => SceneManager.LoadScene(0);
    }
}
