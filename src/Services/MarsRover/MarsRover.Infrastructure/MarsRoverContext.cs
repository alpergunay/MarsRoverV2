using Hb.MarsRover.DataAccess.EntityFramework;
using Hb.MarsRover.Domain;
using Hb.MarsRover.Infrastructure.Core.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using MarsRover.Domain.DomainModels;
using MarsRover.Infrastructure.EntityConfigurations;

namespace MarsRover.Infrastructure
{
    public class MarsRoverContext : BaseDbContext<MarsRoverContext>,
        IUnitOfWork
    {
        public const string DEFAULT_SCHEMA = "marsrover";
        private readonly IMediator _mediator;
        private IDbContextTransaction _currentTransaction;
        public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;
        public bool HasActiveTransaction => _currentTransaction != null;
        public DbSet<Plateau> Plateaus { get; set; }
        public DbSet<Command> Commands { get; set; }
        public DbSet<Direction> Directions { get; set; }

        protected MarsRoverContext(DbContextOptions<MarsRoverContext> options, IUserService userService) : base(options,
            userService)
        {
        }


        public MarsRoverContext() : base(new DbContextOptionsBuilder<MarsRoverContext>()
            .UseNpgsql("Server=localhost;Database=marsrover;User ID=marsrover;Password=123").Options)
        {
        }

        public MarsRoverContext(DbContextOptions<MarsRoverContext> options) : base(options) { }

        public MarsRoverContext(DbContextOptions<MarsRoverContext> options, IMediator mediator, IUserService service)
            : base(options, service)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public override async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await _mediator.DispatchDomainEventsAsync(this);

            // After executing this line all the changes (from the Command Handler and Domain Event Handlers)
            // performed through the DbContext will be committed
            var result = await base.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_currentTransaction != null) return null;

            _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            return _currentTransaction;
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction)
                throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PlateauEntityTypeConfigurations());
            modelBuilder.ApplyConfiguration(new CommandEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new DirectionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RoverEntityTypeConfigurations());
        }
    }
}
