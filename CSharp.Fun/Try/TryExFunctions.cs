using System;

namespace CSharp.Fun
{
    public static class TryExFunctions
    {
        public static Try<B> FlatMap<T, B>(this Try<T> option, Func<T, Try<B>> mapper)
        {
            try
            {
                var failure = option as Failure<T>;
                return failure == null ?
                    mapper(option.Value) :
                    new Failure<B>(failure.Exception);
            }
            catch (Exception ex)
            {
                return new Failure<B>(ex);
            }
        }

        public static Try<B> Map<T, B>(this Try<T> option, Func<T, B> mapper)
        {
            var failure = option as Failure<T>;
            return failure == null ?
                Try.From(mapper(option.Value)) :
                new Failure<B>(failure.Exception);
        }
    }
}
