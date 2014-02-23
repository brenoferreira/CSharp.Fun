using System;
namespace CSharpOptions
{
    public abstract class Option<T>
    {
        public static Option<T> Create()
        {
            return new None<T>();
        }

        public static Option<T> Create(T value)
        {
            if (value == null)
                return new None<T>();

            return new Some<T>(value);
        }

        public readonly T Value;

        public readonly bool HasValue;

        protected Option()
        {
            Value = default(T);
            HasValue = false;
        }

        protected Option(T value)
        {
            Value = value;
            HasValue = true;
        }

        public Option<B> FlatMap<B>(Func<T, Option<B>> func)
        {
            if (HasValue)
                return func(Value);
            return new None<B>();
        }

        public Option<B> Map<B>(Func<T, B> func)
        {
            return FlatMap<B>(x => Option<B>.Create(func(x)));
        }

        public T GetOrElse(T defaultValue)
        {
            if (this is Some<T>)
                return Value;

            return defaultValue;
        }

        public static implicit operator Option<T>(T value)
        {
            return Option<T>.Create(value);
        }
        
        internal class Some<T> : Option<T>
        {
            public Some(T value)
                : base(value)
            { }
        }

        internal class None<T> : Option<T>
        {
            public None()
                : base()
            { }
        }
    }
}
