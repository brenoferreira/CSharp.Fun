namespace CSharp.Fun
{
    public class Option
    {
        private static readonly Option none = new Option();

        private Option() { }

        public static Option None
        {
            get { return none; }
        }

        public static Option<T> From<T>(T value)
        {
            return value == null ? (Option<T>) None : new Some<T>(value);
        }

        public static Option<T> From<T>(T? value) where T : struct
        {
            return value != null ? From(value.Value) : None;
        }
    }
}