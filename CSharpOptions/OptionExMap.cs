using System;

namespace CSharpOptions
{
    public static class OptionExMap
    {
        public static Option<B> FlatMap<T, B>(this Option<T> option, Func<T, Option<B>> func)
        {
            return option.HasValue ? func(option.Value) : Option.None<B>();
        }

        public static Option<B> Map<T, B>(this Option<T> option, Func<T, B> func)
        {
            return FlatMap(option, x => Option.Create(func(x)));
        }

        public static T GetOrElse<T>(this Option<T> option, T defaultValue)
        {
            return option.HasValue ? option.Value : defaultValue;
        }
    }
}
