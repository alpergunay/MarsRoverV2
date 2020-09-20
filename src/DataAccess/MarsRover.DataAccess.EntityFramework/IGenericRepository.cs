using System;
using System.Collections.Generic;
using System.Text;
using Hb.MarsRover.Domain;
using Hb.MarsRover.Domain.Types;

namespace Hb.MarsRover.DataAccess.EntityFramework
{
    public interface IGenericRepository<T> where T : IAggregateRoot<Guid>
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
