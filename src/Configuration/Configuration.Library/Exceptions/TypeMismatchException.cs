using System;

namespace Configuration.Library.Exceptions
{
    public class TypeMismatchException : Exception
    {
        public TypeMismatchException(string message) : base(message)
        {
        }
    }
}