using System;
using System.Collections.Generic;

namespace CSharp.Fun
{

    public abstract class Option<T> : IEquatable<Option<T>>
    {
        public bool HasValue { get; protected set; }

        public T Value { get; protected set; }

        public abstract bool Equals(Option<T> other);

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

        public static implicit operator Option<T> (Option option)
        {
            return new None<T>();
        }
    }

    internal class Some<T> : Option<T>
    {
        public Some(T value)
        {
            Value = value;
            HasValue = true;
        }

        public override bool Equals(Option<T> other)
        {
            return other.HasValue && Value.Equals(other.Value);
        }
    }

    internal class None<T> : Option<T>
    {
        public new T Value { get { throw new Exception("No value"); } }

        public None()
        {
            HasValue = false;
        }

        public override bool Equals(Option<T> other)
        {
            return !other.HasValue;
        }

        public override bool Equals(object obj)
        {
            if (obj is Option)
                return true;

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}
