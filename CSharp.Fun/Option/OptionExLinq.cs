using System;

namespace CSharp.Fun
{
    public static class OptionExLinq
    {
        public static Option<B> SelectMany<T, B>(this Option<T> option, Func<T, Option<B>> func)
        {
            return option.FlatMap(func);
        }

        public static Option<B> Select<T, B>(this Option<T> option, Func<T, B> selector)
        {
            return option.Map(selector);
        }

        public static Option<V> SelectMany<T, U, V>(this Option<T> option, Func<T, Option<U>> selector, Func<T, U, V> resultSelector)
        {
            return option.SelectMany(x => selector(x).SelectMany(y => Option.From(resultSelector(x, y))));
        }

        public static Option<T> Where<T>(this Option<T> option, Func<T, Boolean> predicate)
        {
            return option.Filter(predicate);
        }

    }
}
