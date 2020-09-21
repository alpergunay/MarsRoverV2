using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Hb.MarsRover.DataAccess.EntityFramework;
using Hb.MarsRover.Domain;
using MarsRover.Domain.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace MarsRover.Infrastructure.Repositories
{
    public class PlateauRepository : IPlateauRepository
    {
        private readonly MarsRoverContext _context;
        public PlateauRepository(MarsRoverContext context, IMapper mapper) 
        {
            _context = context;
        }
        public IUnitOfWork UnitOfWork => _context;

        public Plateau Add(Plateau entity)
        {
            _context.Plateaus.Add(entity);
            return entity;
        }

        public async Task<Plateau> FindOrDefaultAsync(Guid entityId)
        {
            return await _context.Plateaus.FirstOrDefaultAsync(p => p.Id == entityId);
        }
    }
}
