using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scripts
{
    public class LevelLoading : MonoBehaviour
    {
        [SerializeField] 
        private LoadingScreen loadScreen;

        public void LoadLevelAsync()
        {
            var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            StartCoroutine(LoadLevelAsync_COR(currentSceneIndex != SceneManager.sceneCountInBuildSettings - 1
                                                ? currentSceneIndex + 1
                                                : 0));
        }

        public IEnumerator LoadLevelAsync_COR(int sceneIndex)
        {
            if (sceneIndex != 0)
            {
                loadScreen.SetBackground(Resources.Load<Sprite>("Load Screens/LoadScreen_Level" + sceneIndex));
            }
            yield return StartCoroutine(loadScreen.Show());

            var operation = SceneManager.LoadSceneAsync(sceneIndex);
            while (!operation.isDone)
            {
                loadScreen.SetLoadingBarAppearance(operation.progress, string.Format(@"Загрузка... {0}%", Mathf.Round(operation.progress * 100)));
                yield return null;
            }
        }
    }
}
