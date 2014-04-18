using System;

namespace CSharp.Fun
{
    public interface IOption<out T>
    {
        bool HasValue { get; }

        T Value { get; }
    }

    public abstract class Option<T> : IOption<T>
    {
        public bool HasValue { get; protected set; }
        public T Value { get; protected set; }

        public static implicit operator Option<T>(Option option)
        {
            return new None<T>();
        }
    }

    class Some<T> : Option<T>, IEquatable<Option<T>>
    {
        public Some(T value)
        {
            Value = value;
            HasValue = true;
        }

        public bool Equals(Option<T> other)
        {
            return other.HasValue && other.Value.Equals(Value);
        }
    }

    class None<T> : Option<T>, IEquatable<IOption<T>>
    {
        public new T Value { get { throw new Exception("No value"); } }

        public None()
        {
            HasValue = false;
        }

        public bool Equals(IOption<T> other)
        {
            return !other.HasValue;
        }
    }
}
