using System.Collections.Generic;
using KellermanSoftware.CompareNetObjects;
using System;


namespace BirthdayGreetings.Core.Test.Extension
{
    internal static class EnumerableExtension
    {
        internal static void CompareItemValuesIgnoringOrder<T>(this IEnumerable<T> first, IEnumerable<T> second, Action onEquality, Action<string> onDifference)
        {
            var compareLogic = new CompareLogic(new ComparisonConfig { IgnoreCollectionOrder = true });
            var comparisonResult = compareLogic.Compare(first, second);
            if (comparisonResult.AreEqual)
                onEquality();
            else
                onDifference(comparisonResult.DifferencesString);
        }
    }
}