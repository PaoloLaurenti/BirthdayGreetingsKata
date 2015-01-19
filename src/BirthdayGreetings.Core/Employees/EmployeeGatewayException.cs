using System;
using System.Runtime.Serialization;

namespace BirthdayGreetings.Core.Employees
{
    public class EmployeeGatewayException : Exception
    {
        public EmployeeGatewayException()
        {
            
        }

        public EmployeeGatewayException(string message)
            : base(message)
        {
        }

        public EmployeeGatewayException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public EmployeeGatewayException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}