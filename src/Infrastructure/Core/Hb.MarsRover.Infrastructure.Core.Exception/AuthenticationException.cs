namespace Hb.MarsRover.Infrastructure.Core.Exception
{
    public sealed class AuthenticationException : System.Exception
    {
        public AuthenticationException()
        {
        }

        public AuthenticationException(string msg)
            : base(msg)
        {
        }
    }
}