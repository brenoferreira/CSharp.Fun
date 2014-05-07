using System;
using System.Collections.Generic;

namespace CSharp.Fun
{
    public abstract class Option<T>
    {
        public bool HasValue { get; protected set; }

        public T Value { get; protected set; }

        public static implicit operator Option<T> (Option option)
        {
            return new None<T>();
        }
    }

    internal class Some<T> : Option<T>, IEquatable<Option<T>>
    {
        public Some(T value)
        {
            Value = value;
            HasValue = true;
        }

        public bool Equals(Option<T> other)
        {
            return other.HasValue && Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if(obj is Option<T>) return Equals(obj as Option<T>);

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (HasValue.GetHashCode() * 397) ^ EqualityComparer<T>.Default.GetHashCode(Value);
            }
        }
    }

    internal class None<T> : Option<T>, IEquatable<Option<T>>
    {
        public new T Value { get { throw new Exception("No value"); } }

        public None()
        {
            HasValue = false;
        }

        public bool Equals(Option<T> other)
        {
            return !other.HasValue;
        }

        public override bool Equals(object obj)
        {
            return obj is Option;
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}
