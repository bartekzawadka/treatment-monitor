using System;

namespace Treatment.Monitor.BusinessLogic.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        private readonly TimeZoneInfo _polishTimeZoneInfo;
        
        public DateTimeProvider()
        {
            var isWindows = Environment.OSVersion.Platform is PlatformID.Win32S 
                or PlatformID.Win32Windows
                or PlatformID.Win32NT
                or PlatformID.WinCE;
            _polishTimeZoneInfo = isWindows
                ? TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time")
                : TimeZoneInfo.FindSystemTimeZoneById("Europe/Warsaw");
        }

        public TimeZoneInfo GetTimeZoneInfo() => _polishTimeZoneInfo;
        
        public DateTime ConvertDateToPolishTimeZone(DateTime dateTime) => 
            TimeZoneInfo.ConvertTimeFromUtc(dateTime.ToUniversalTime(), _polishTimeZoneInfo);
    }
}