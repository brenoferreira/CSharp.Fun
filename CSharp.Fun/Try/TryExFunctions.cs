using System;

namespace CSharp.Fun
{
    public static class TryExFunctions
    {
        private static readonly Try<Unit> unitTry = Try.From(Unit.Instance);

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

        public static Try<Unit> Map<T>(this Try<T> tryValue, Action<T> mapper)
        {
            return FlatMap(tryValue, (val) =>
            {
                mapper(val);
                return unitTry;
            });
        }

        public static Try<T> Filter<T>(this Try<T> tryValue, Func<T, Boolean> predicate)
        {
            try
            {
                if (!tryValue.IsSuccess) return tryValue;
                
                return predicate(tryValue.Value)
                    ? tryValue
                    : new Failure<T>(new NoSuchElementException("Predicate does not hold for " + tryValue.Value));
            }
            catch (Exception ex)
            {
                return new Failure<T>(ex);
            }
        }
    }
}
