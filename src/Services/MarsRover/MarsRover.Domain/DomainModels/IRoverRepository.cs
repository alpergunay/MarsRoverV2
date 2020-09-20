using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Hb.MarsRover.DataAccess.EntityFramework;
using Hb.MarsRover.Domain;

namespace MarsRover.Domain.DomainModels
{
    public interface IRoverRepository : IGenericRepository<Rover>
    {
        Rover Add(Rover entity);
        Task<Rover> FindOrDefaultAsync(Guid entityId);
        void Update(Rover entity);
    }
}
