using System;

namespace CSharp.Fun
{
    public class Option : IEquatable<Option>
    {
        private static readonly Option NoneSingleton = new Option();
        public static Option None
        {
            get { return NoneSingleton; }
        }

        private Option() { }

        public bool Equals(Option other)
        {
            return true;
        }

        public static Option<T> From<T>(T value)
        {
            return value != null 
                ? new Some<T>(value)
                : (Option<T>) None;
        }

        public static Option<T> From<T>(T? value) where T : struct
        {
            return value != null 
                ? From(value.Value) 
                : None;
        }
    }
}