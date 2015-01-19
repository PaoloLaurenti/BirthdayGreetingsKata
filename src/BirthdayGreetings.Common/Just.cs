using System;

namespace BirthdayGreetings.Common
{
    public class Just<T> : IMaybe<T>
    {
        public Just(T value)
        {
            Value = value;
        }

        public T Value { get; private set; }

        public IMaybe<U> Map<U>(Func<T, IMaybe<U>> func)
        {
            return func(Value);
        }

        public IMaybe<T> Do(Action<T> action)
        {
            action(Value);
            return this;
        }

        public IMaybe<T> DoIf(Predicate<T> predicate, Action<T> action)
        {
            if (predicate(Value))
                action(Value);
            return this;
        }

        public IMaybe<T> Do(Action<IMaybe<T>> action)
        {
            action(this);
            return this;
        }
    }
}