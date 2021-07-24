using System;

namespace Treatment.Monitor.BusinessLogic.Services
{
    public interface IDateTimeProvider
    {
        TimeZoneInfo GetTimeZoneInfo();

        DateTime ConvertDateToPolishTimeZone(DateTime dateTime);
    }
}