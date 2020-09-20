namespace Hb.MarsRover.Infrastructure.Core.Exception
{
    public sealed class ConflictException : System.Exception
    {
        public ConflictException()
        {
        }

        public ConflictException(string msg)
            : base(msg)
        {
        }
    }
}