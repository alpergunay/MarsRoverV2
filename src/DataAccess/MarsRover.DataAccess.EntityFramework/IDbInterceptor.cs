using Hb.MarsRover.DataAccess.EntityFramework.Interceptors;

namespace Hb.MarsRover.DataAccess.EntityFramework
{
    public interface IDbInterceptor
    {
        void Before(DbInterceptionContext context);

        void After(DbInterceptionContext context);
    }
}