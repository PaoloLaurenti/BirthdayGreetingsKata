using System;

namespace BirthdayGreetings.Common
{
    public class Nothing<T> : IMaybe<T>
    {
        public IMaybe<U> Map<U>(Func<T, IMaybe<U>> func)
        {
            return new Nothing<U>();
        }

        public IMaybe<T> Do(Action<T> action)
        {
            return this;
        }

        public IMaybe<T> DoIf(Predicate<T> predicate, Action<T> action)
        {
            return this;
        }

        public IMaybe<T> Do(Action<IMaybe<T>> action)
        {
            return this;
        }
    }
}