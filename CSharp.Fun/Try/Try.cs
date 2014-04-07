using System;
using System.Collections.Generic;

namespace CSharp.Fun
{
    public interface Try<out T>
    {
        bool IsSuccess { get; }

        T Value { get; }
    }

    public sealed class Failure<T> : Try<T>, IEquatable<Failure<T>>
    {
        private readonly Exception _exception;

        internal Failure(Exception exception)
        {
            _exception = exception;
        }

        public bool IsSuccess { get { return false; } }

        public T Value { get { throw _exception; } }

        public Exception Exception { get { return _exception; } }

        public bool Equals(Failure<T> other)
        {
            return other.Exception.Equals(Exception);
        }

        public override bool Equals(object obj)
        {
            if (obj is Failure<T>) return Equals(obj as Failure<T>);
            return false;
        }

        public override int GetHashCode()
        {
            return (_exception != null ? _exception.GetHashCode() : 0);
        }
    }

    public sealed class Success<T> : Try<T>, IEquatable<Success<T>>
    {
        private readonly T _value;

        internal Success(T value)
        {
            _value = value;
        }

        public bool IsSuccess { get { return true; } }

        public T Value { get { return _value; } }

        public bool Equals(Success<T> other)
        {
            return other.Value.Equals(Value);
        }

        public override bool Equals(object obj)
        {
            if (obj is Success<T>) return Equals(obj as Success<T>);
            return false;
        }

        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(_value);
        }
    }
}
