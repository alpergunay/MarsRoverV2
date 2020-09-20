using Hb.MarsRover.DataAccess.EntityFramework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MarsRover.Domain.DomainModels
{
    public interface IPlateauRepository : IGenericRepository<Plateau>
    {
        Plateau Add(Plateau entity);

        Task<Plateau> FindOrDefaultAsync(Guid entityId);
    }
}
