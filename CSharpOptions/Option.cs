using System;
namespace CSharpOptions
{
    public interface Option<T>
    {
        bool HasValue { get; }

        T Value { get; }
    }

    public static class Option
    {
        public static Option<T> Create<T>(T value)
        {
            if (value == null) return default(None<T>);
            return new Some<T>(value);
        }

        public static Option<T> None<T>()
        {
            return default(None<T>);
        }
    }


    struct Some<T> : Option<T>
    {
        public Some(T value) : this()
        {
            Value = value;
            HasValue = true;
        }

        public bool HasValue { get; private set; }
        public T Value { get; private set; }
    }

    struct None<T> : Option<T>
    {
        public bool HasValue { get { return false; } }
        public T Value { get { throw new Exception("No value"); } }
    }
}
