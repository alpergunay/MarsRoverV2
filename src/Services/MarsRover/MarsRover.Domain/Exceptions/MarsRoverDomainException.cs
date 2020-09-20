using Hb.MarsRover.Infrastructure.Core.Exception;
using System;

namespace MarsRover.Domain.Exceptions
{
    /// <summary>
    /// Exception type for domain exceptions
    /// </summary>
    public class MarsRoverDomainException : DomainException
    {
        public MarsRoverDomainException()
        { }

        public MarsRoverDomainException(string message)
            : base(message)
        { }

        public MarsRoverDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
