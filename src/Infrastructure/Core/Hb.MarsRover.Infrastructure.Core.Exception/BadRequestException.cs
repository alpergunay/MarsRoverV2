namespace Hb.MarsRover.Infrastructure.Core.Exception
{
    public class BadRequestException : System.Exception
    {
        public BadRequestException()
        {
        }

        public BadRequestException(string message) : base(message)
        {
        }
    }
}