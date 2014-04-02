using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharp.Fun.Try
{
    public static class Try
    {
        public static Try<T> From<T>(Func<T> throwable)
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
        }
    }
}
