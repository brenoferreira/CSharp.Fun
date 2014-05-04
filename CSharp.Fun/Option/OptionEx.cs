namespace CSharp.Fun
{
    public static class OptionEx
    {
        public static Option<T> ToOption<T>(this T value)
        {
            return Option.From(value);
        }

        public static Option<T> ToOption<T>(this T? value) where T : struct
        {
            return Option.From(value);
        }
    }
}
