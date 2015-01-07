using System;

namespace BirthdayGreetings.Core.Test.Extension
{
    internal static class DateTimeExtension
    {
        internal static DateTime GetANewDateWithDifferentDay(this DateTime date)
        {
            var r = new Random(DateTime.Now.Second);
            var yearToSubstract = r.Next(-50, -20);
            var daysToSubstract = r.Next(-10, -1);
            return date.AddYears(yearToSubstract).AddDays(daysToSubstract);
        }
    }
}