namespace Hb.MarsRover.Infrastructure.Core.Exception
{
    public sealed class AuthorizationException : System.Exception
    {
        public AuthorizationException()
        {
        }

        public AuthorizationException(string msg)
            : base(msg)
        {
        }
    }
}