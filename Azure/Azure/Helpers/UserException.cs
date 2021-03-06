using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Helpers
{
    [Serializable]
    public class UserException : Exception
    {
        public UserException()
        {
        }

        public UserException(string message) : base(message)
        {
        }

        public UserException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
