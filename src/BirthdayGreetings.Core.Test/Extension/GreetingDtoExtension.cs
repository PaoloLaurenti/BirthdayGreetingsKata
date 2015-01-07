using System.Collections.Generic;
using System.Linq;
using BirthdayGreetings.Core.Greetings;

namespace BirthdayGreetings.Core.Test.Extension
{
    internal static class GreetingDtoExtension
    {
        internal static IEnumerable<GreetingDto> Sort(this IEnumerable<GreetingDto> greetings)
        {
            return greetings.OrderBy(y => string.Format("{0}-{1}", y.FirstName, y.Email));
        }
    }
}