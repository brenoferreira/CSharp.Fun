using System;

namespace CSharp.Fun
{
    public static class Try
    {
        public static Try<Unit> From(Action throwable)
        {
            return From(() =>
            {
                throwable();
                return Unit.Instance;
            });
        }

        public static Try<T> From<T>(Func<T> throwable)
        {
            return From(throwable, null);
        }

        public static Try<T> From<T>(Func<T> throwable, Action finallyAction)
        {
            try
            {
                var value = throwable();

                return new Success<T>(value);
            }
            catch (Exception ex)
            {
                return new Failure<T>(ex);
            }
            finally
            {
                if(finallyAction != null) finallyAction();
            }
        }
    }
}
