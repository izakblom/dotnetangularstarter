using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using dotnetstarter.gateway.domain.AggregatesModel.UserAggregate;

namespace dotnetstarter.gateway.infrastructure.EntityConfigurations
{

    class UserConfiguration
        : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> conf)
        {
            conf.ToTable("Users", PortalContext.DEFAULT_SCHEMA);

            conf.HasKey(b => b.Id);

            conf.Ignore(b => b.DomainEvents);

            conf.Property(b => b.Id)
                .ForSqlServerUseSequenceHiLo("buyerseq", PortalContext.DEFAULT_SCHEMA);

            conf.Property<string>("FirstName");
            conf.Property<string>("LastName");
            conf.Property<string>("IdentityNumber");
            conf.Property<string>("MobileNumber");
            conf.Property<bool>("IsActive").HasDefaultValue(true);
            conf.Property<string>("Email");
            conf.Property<bool>("Verified");

            conf.Property(b => b.JWTId)
                .HasMaxLength(200)
                .IsRequired();

            conf.HasIndex("JWTId")
              .IsUnique(true);


            conf.HasIndex("Email").IsUnique(false);
        }
    }
}
