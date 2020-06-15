using System;
using System.Runtime.Serialization;

namespace Microsoft.Extensions.Configuration
{
    /// <summary>
    /// The exception that is thrown,
    /// when a value could not be resolved.
    /// </summary>
    public class ValueUnresolvableException : InvalidOperationException
    {
        internal ValueUnresolvableException()
        {
        }

        internal ValueUnresolvableException(string message) : base(message)
        {
        }

        internal ValueUnresolvableException(string message, Exception innerException) : base(message, innerException)
        {
        }

        internal ValueUnresolvableException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
