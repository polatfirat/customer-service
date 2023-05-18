using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer.Domain.Exceptions
{
    public class ErrorResult
    {
        public int ErrorCode { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Source { get; set; }
    }
    public class CustomerDatabaseException : Exception
    {
        public CustomerDatabaseException(string message) : base(message)
        {

        }

        public CustomerDatabaseException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }

    public class CustomerOperationException : Exception
    {
        public CustomerOperationException(string message) : base(message)
        {

        }

        public CustomerOperationException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }

    public class CustomerValidationException : Exception
    {
        public CustomerValidationException(string message) : base(message)
        {

        }

        public CustomerValidationException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
