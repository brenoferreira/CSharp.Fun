using System;

namespace CSharp.Fun
{
    public static class TryExRecovery
    {
        public static Try<B> Recover<T, B>(this Try<T> tryValue, Func<Exception, B> recovery) where B : T
        {
            if (tryValue.IsSuccess) return tryValue as Try<B>;

            try
            {
                var failure = tryValue as Failure<T>;

                return Try.From(recovery(failure.Exception));
            }
            catch (Exception ex)
            {
                return new Failure<B>(ex);
            }
        }

        public static Try<B> RecoverWith<T, B>(this Try<T> tryValue, Func<Exception, Try<B>> recovery) where B : T
        {
            if (tryValue.IsSuccess) return tryValue as Try<B>;

            try
            {
                var failure = tryValue as Failure<T>;

                return recovery(failure.Exception);
            }
            catch (Exception ex)
            {
                return new Failure<B>(ex);
            }
        }
    }
}
