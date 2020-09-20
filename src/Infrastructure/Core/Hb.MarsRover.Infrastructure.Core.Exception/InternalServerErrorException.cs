namespace Hb.MarsRover.Infrastructure.Core.Exception
{
    public class InternalServerErrorException : System.Exception
    {
        public InternalServerErrorException()
        {
        }

        public InternalServerErrorException(string msg)
            : base(msg)
        {
        }
    }
}