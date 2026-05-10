namespace GameLogic
{
    public class CodingTaskModel
    {
        public readonly CodingTaskConfig Config;

        public string PlayerCode;
        public int TaskCompletingTimeInSeconds;
        public int UsedTipsCount;

        public CodingTaskModel(CodingTaskConfig config)
        {
            Config = config;
        }
    }
}
