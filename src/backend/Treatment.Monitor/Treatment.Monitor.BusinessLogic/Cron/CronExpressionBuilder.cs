namespace Treatment.Monitor.BusinessLogic.Cron
{
    public class CronExpressionBuilder
    {
        private string _minute = "*";
        private string _hour = "*";
        
        public CronExpressionBuilder WithMinute(int minute)
        {
            _minute = minute.ToString();
            return this;
        }

        public CronExpressionBuilder WithHour(int hour)
        {
            _hour = hour.ToString();
            return this;
        }

        public string BuildExpression()
        {
            return $"{_minute} {_hour} * * *";
        }
    }
}