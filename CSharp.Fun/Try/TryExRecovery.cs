using System;

namespace CSharp.Fun
{
    public static class TryExRecovery
    {
		public static Try<B> Recover<T, TException, B>(this Try<T> tryValue, Func<TException, B> recovery) where B : T where TException : Exception
		{
			if (tryValue.IsSuccess) return tryValue as Try<B>;

			try
			{
				var failure = tryValue as Failure<T>;
				var ex = failure.Exception as TException;

				return ex != null ? Try.From(recovery(ex)) : tryValue as Try<B>;
			}
			catch (Exception ex)
			{
				return new Failure<B>(ex);
			}
		}

		public static Try<B> RecoverWith<T, TException, B>(this Try<T> tryValue, Func<TException, Try<B>> recovery) where B : T where TException : Exception
        {
            if (tryValue.IsSuccess) return tryValue as Try<B>;

            try
            {
				var failure = tryValue as Failure<T>;
				var ex = failure.Exception as TException;

				return ex != null ? recovery(ex) : tryValue as Try<B>;
            }
            catch (Exception ex)
            {
                return new Failure<B>(ex);
            }
        }
    }
}
