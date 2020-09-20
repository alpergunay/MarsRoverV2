using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Hb.MarsRover.DataAccess.EntityFramework;
using Hb.MarsRover.Domain;
using MarsRover.Domain.DomainModels;

namespace MarsRover.Infrastructure.Repositories
{
    public class PlateauRepository : GenericRepository<MarsRoverContext, Plateau, Guid>, IPlateauRepository
    {
        private readonly MarsRoverContext _context;
        public PlateauRepository(MarsRoverContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
        }
        public IUnitOfWork UnitOfWork => _context;
    }
}
