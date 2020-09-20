using System;
using Hb.MarsRover.Infrastructure.Core.Exception;

namespace MarsRover.Domain.Exceptions
{
    public class PlateauDomainException : DomainException
    {
        public PlateauDomainException()
        { }

        public PlateauDomainException(string message)
            : base(message)
        { }

        public PlateauDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
