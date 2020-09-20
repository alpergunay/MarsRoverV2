namespace Hb.MarsRover.Infrastructure.Core.Exception
{
    public class ApiValidationException : BadRequestException
    {
        public ApiValidationException(string message) : base(message)
        {
        }
    }
}