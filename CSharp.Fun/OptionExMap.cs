using System;

namespace CSharp.Fun
{
    public static class OptionExMap
    {
        public static Option<B> FlatMap<T, B>(this Option<T> option, Func<T, Option<B>> mapper)
        {
            return option.HasValue ? mapper(option.Value) : Option.None<B>();
        }

        public static Option<B> Map<T, B>(this Option<T> option, Func<T, B> mapper)
        {
            return FlatMap(option, x => Option.From(mapper(x)));
        }

        public static Option<T> Filter<T>(this Option<T> option, Func<T, Boolean> predicate)
        {
            if (!option.HasValue) return Option.None<T>();
            return predicate(option.Value) ? option : Option.None<T>();
        }

        public static T GetOrElse<T>(this Option<T> option, T defaultValue)
        {
            return option.HasValue ? option.Value : defaultValue;
        }
    }
}
