using dotnetstarter.authentication.domain.AggregatesModel.IdentityAggregate;
using dotnetstarter.authentication.domain.SeedWork;
using dotnetstarter.authentication.infrastructure.EntityConfigurations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace dotnetstarter.authentication.infrastructure
{
    public class IdentityContext : DbContext, IUnitOfWork
    {
        public const string DEFAULT_SCHEMA = "auth";
        public const string LOOKUP_SCHEMA = "lut";

        public DbSet<Identity> Users { get; set; }
        public DbSet<IdentityClaim> UserClaims { get; set; }
        public DbSet<ClaimType> Claimtypes { get; set; }

        private readonly IMediator _mediator;

        private IdentityContext(DbContextOptions<IdentityContext> options) : base(options) { }

        public IdentityContext(DbContextOptions<IdentityContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));


            System.Diagnostics.Debug.WriteLine("IdentityContext::ctor ->" + this.GetHashCode());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new IdentityEntityTypeConfiguration());
            new IdentitySeed(ref modelBuilder);

            modelBuilder.ApplyConfiguration(new ClaimTypeEntityTypeConfiguration());
            new ClaimTypesSeed(ref modelBuilder);

            modelBuilder.ApplyConfiguration(new IdentityClaimEntityTypeConfiguration());
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            // Dispatch Domain Events collection. 
            // Choices:
            // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
            // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
            // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
            // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
            await _mediator.DispatchDomainEventsAsync(this);

            // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
            // performed throught the DbContext will be commited
            var result = await base.SaveChangesAsync();

            return true;
        }

    }

    public class IdentityContextDesignFactory : IDesignTimeDbContextFactory<IdentityContext>
    {
        public IdentityContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<IdentityContext>()
                .UseSqlServer("Server=.;Initial Catalog=example_db;Integrated Security=true");

            return new IdentityContext(optionsBuilder.Options, new NoMediator());

        }

        class NoMediator : IMediator
        {
            public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default(CancellationToken)) where TNotification : INotification
            {
                return Task.CompletedTask;
            }

            public Task Publish(object notification, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.CompletedTask;
            }

            public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult<TResponse>(default(TResponse));
            }

            public Task Send(IRequest request, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.CompletedTask;
            }
        }
    }
}
