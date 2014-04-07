using System;

namespace CSharp.Fun
{
    public static class TryExLinq
    {
        public static Try<B> Select<T, B>(this Try<T> tryValue, Func<T, B> selector)
        {
            return tryValue.Map(selector);
        }
    }
}
