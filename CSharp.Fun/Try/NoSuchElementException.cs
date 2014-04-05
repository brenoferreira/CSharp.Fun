using System;

namespace CSharp.Fun
{
    public class NoSuchElementException : Exception
    {
        public NoSuchElementException(string msg) : base(msg)
        {
            
        }
    }
}