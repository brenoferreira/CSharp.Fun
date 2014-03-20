using System;

namespace CSharpOptions
{
    public static class Option
    {
        public static Option<T> From<T>(T value)
        {
            return value == null ? None<T>() : new Some<T>(value);
        }

        public static Option<T> None<T>()
        {
            return default(None<T>);
        }

        public static Option<T> From<T>(T? value) where T : struct
        {
            return value != null ? From(value.Value) : None<T>();
        }

        public static Option<T> ToOption<T>(this T value)
        {
            return From(value);
        }

        public static Option<T> ToOption<T>(this T? value) where T : struct
        {
            return From(value);
        }
    }
}