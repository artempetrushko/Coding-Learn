using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts
{
    public class SceneLoadingManager : MonoBehaviour
    {
        [SerializeField] 
        private LoadingScreenView loadingScreenView;

        public async UniTask LoadNextSceneAsync(int sceneIndex)
        {
            if (sceneIndex > 0)
            {
                loadingScreenView.gameObject.SetActive(true);
                loadingScreenView.SetBackground(GameContentManager.GetLoadingScreenBackground(sceneIndex + 1));
                await loadingScreenView.ShowAsync();

                var operation = SceneManager.LoadSceneAsync(sceneIndex);
                while (!operation.isDone)
                {
                    loadingScreenView.SetLoadingBarState(operation.progress);
                    await UniTask.Yield();
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
