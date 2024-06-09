using System;

namespace Configuration.Library.Exceptions
{
    public class ConfigNotFoundException : Exception
    {
        public ConfigNotFoundException(string key) : base(key)
        {
        }
    }
}