using System;

namespace CSharp.Fun
{
    public static class TryExLinq
    {
        public static Try<B> SelectMany<T, B>(this Try<T> tryValue, Func<T, Try<B>> selector)
        {
            return tryValue.FlatMap(selector);
        }

        public static Try<V> SelectMany<T, U, V>(this Try<T> tryValue, Func<T, Try<U>> selector, Func<T, U, V> resultSelector)
        {
            return tryValue.SelectMany(x => selector(x).SelectMany(y => Try.From(resultSelector(x, y))));
        }

        public static Try<B> Select<T, B>(this Try<T> tryValue, Func<T, B> selector)
        {
            return tryValue.Map(selector);
        }

        public static Try<T> Where<T>(this Try<T> tryValue, Func<T, Boolean> predicate)
        {
            return tryValue.Filter(predicate);
        }
    }
}
