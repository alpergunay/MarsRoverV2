using System.Threading.Tasks;

namespace Hb.MarsRover.DataAccess.Core
{
    public interface IInitializer
    {
        Task InitializeAsync();
    }
}
