using Cysharp.Threading.Tasks;

namespace Scripts
{
    public class TaskDescriptionSectionController 
    {
        private const float VISIBILITY_CHANGING_DURATION = 1f;

        private TaskDescriptionSectionView _taskDescriptionSectionView;

        public TaskDescriptionSectionController(TaskDescriptionSectionView taskDescriptionSectionView)
        {
            _taskDescriptionSectionView = taskDescriptionSectionView;
        }

        public void SetContent(string taskTitle, string taskDesription)
        {
            _taskDescriptionSectionView.SetTaskTitleText(taskTitle);
            _taskDescriptionSectionView.SetTaskDescriptionText(taskDesription);
            _taskDescriptionSectionView.SetScrollbarValue(1);
        }

        public async UniTask SetVisibilityAsync(bool isVisible) => await _taskDescriptionSectionView.SetVisibilityAsync(isVisible, VISIBILITY_CHANGING_DURATION);
    }
}
