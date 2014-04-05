using System;

namespace CSharp.Fun
{
    public static class TryExFunctions
    {
        public static Try<B> FlatMap<T, B>(this Try<T> tryValue, Func<T, Try<B>> mapper)
        {
            try
            {
                var failure = tryValue as Failure<T>;
                return failure == null ?
                    mapper(tryValue.Value) :
                    new Failure<B>(failure.Exception);
            }
            catch (Exception ex)
            {
                return new Failure<B>(ex);
            }
        }

        public static Try<B> Map<T, B>(this Try<T> tryValue, Func<T, B> mapper)
        {
            var failure = tryValue as Failure<T>;
            return failure == null ?
                Try.From(mapper(tryValue.Value)) :
                new Failure<B>(failure.Exception);
        }
    }
}
