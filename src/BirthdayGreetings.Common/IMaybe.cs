using System;

namespace BirthdayGreetings.Common
{
    public interface IMaybe<T>
    {
        IMaybe<U> Map<U>(Func<T, IMaybe<U>> func);
        IMaybe<T> Do(Action<T> action);
        IMaybe<T> DoIf(Predicate<T> predicate, Action<T> action);
        IMaybe<T> Do(Action<IMaybe<T>> action);
    }
}