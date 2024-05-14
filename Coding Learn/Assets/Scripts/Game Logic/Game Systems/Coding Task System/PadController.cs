namespace Scripts
{
    public class PadController
    {
        private DevEnvironmentController devEnvironmentManager;
        private IPadSecondaryFunction[] secondaryFunctions;     

        public PadController(DevEnvironmentController devEnvironmentManager, IPadSecondaryFunction[] secondaryFunctions)
        {
            this.devEnvironmentManager = devEnvironmentManager;
            this.secondaryFunctions = secondaryFunctions;
        }

        public void SetCurrentTaskContent(TaskContent taskContent)
        {

        }
    }
}
