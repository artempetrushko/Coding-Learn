namespace GameLogic
{
    public class DevEnvironmentModel
    {
        public readonly string StartCode;
        public readonly string TestCode;
        public readonly string TestMethodName;
        public readonly string PlayerCodePlaceholder;

        public DevEnvironmentModel(string startCode, string testCode, string testMethodName, string playerCodePlaceholder)
        {
            StartCode = startCode;
            TestCode = testCode;
            TestMethodName = testMethodName;
            PlayerCodePlaceholder = playerCodePlaceholder;
        }
    }
}
