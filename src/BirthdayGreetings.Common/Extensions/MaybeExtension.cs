namespace BirthdayGreetings.Common.Extensions
{
    public static class MaybeExtension
    {
        public static IMaybe<T> ToMaybe<T>(this T value)
        {
            return value == null ? (IMaybe<T>) new Nothing<T>() : new Just<T>(value);
        }
    }
}