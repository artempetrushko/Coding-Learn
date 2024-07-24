namespace Scripts
{
    public class TaskDescriptionSectionController 
    {
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
    }
}
