using System.Collections.Generic;
using KellermanSoftware.CompareNetObjects;
using System;


namespace BirthdayGreetings.Core.Test.Extension
{
    internal static class EnumerableExtension
    {
        internal static void CompareAlreadySortedItems<T>(this List<T> first, List<T> second, Action onEquality, Action<string> onDifference)
        {
            var compareLogic = new CompareLogic();
            var comparisonResult = compareLogic.Compare(first, second);
            if (comparisonResult.AreEqual)
                onEquality();
            else
                onDifference(comparisonResult.DifferencesString);
        }
    }
}