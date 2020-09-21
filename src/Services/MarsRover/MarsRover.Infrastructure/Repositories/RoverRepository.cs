using AutoMapper;
using Hb.MarsRover.Domain;
using MarsRover.Domain.DomainModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Hb.MarsRover.DataAccess.EntityFramework;

namespace MarsRover.Infrastructure.Repositories
{
    public class RoverRepository : GenericRepository<MarsRoverContext, Rover, Guid>, IRoverRepository
    {
        private readonly MarsRoverContext _context;
        public RoverRepository(MarsRoverContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
        }
        public IUnitOfWork UnitOfWork => _context;

        public override Rover Add(Rover entity)
        {
            //TODO: First line should be removed with refactoring
            _context.Entry(entity.Direction).State = EntityState.Unchanged;
            _context.Rovers.AddAsync(entity);
            return entity;
        }
        public override async Task<Rover> FindOrDefaultAsync(Guid entityId)
        {
            var rover = await _context.Rovers.Include(r => r.Plateau)
                .FirstOrDefaultAsync(r => r.Id == entityId);
            return rover;
        }

        public override void Update(Rover entity)
        {
            _context.Entry(entity.Direction).State = EntityState.Unchanged;
            _context.Rovers.Update(entity);
        }
    }
}
