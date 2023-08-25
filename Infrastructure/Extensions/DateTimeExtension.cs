using System;

namespace Infrastructure.Extensions
{
    public static class DateTimeExtension
    {
        public static string GetDate(this DateTime dateTime, char? separator = null)
        {
            string day = dateTime.Day.ToString().PadLeft(2, '0');
            string month = dateTime.Month.ToString().PadLeft(2, '0');
            string year = dateTime.Year.ToString();

            if (separator == null)
            {
                return $"{day}{month}{year}";
            }
            return $"{day}{separator}{month}{separator}{year}";
        }
    }
}
