using UnityEngine;

namespace UI.MainMenu
{
    public class TaskStatisticsPageView : MonoBehaviour
    {
        [SerializeField] private GameObject _taskStatisticsViewsContainer;

        public GameObject TaskStatisticsViewsContainer => _taskStatisticsViewsContainer;

        public void SetActive(bool isActive) => gameObject.SetActive(isActive);
    }
}
