using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using dotnetstarter.organisations.domain.AggregatesModel.UserAggregate;

namespace dotnetstarter.organisations.infrastructure.EntityConfigurations
{

    class UserConfiguration
        : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> conf)
        {
            conf.ToTable("Users", OrganisationsContext.DEFAULT_SCHEMA);

            conf.HasKey(b => b.Id);

            conf.Ignore(b => b.DomainEvents);

            conf.Property(b => b.Id)
                .ForSqlServerUseSequenceHiLo("buyerseq", OrganisationsContext.DEFAULT_SCHEMA);

            conf.Property<string>("FirstName");
            conf.Property<string>("LastName");
            conf.Property<string>("IdentityNumber");
            conf.Property<string>("MobileNumber");
            conf.Property<bool>("IsActive").HasDefaultValue(true);
            conf.Property<string>("Email");
            conf.Property<int>("CoreUserIdRef");



            conf.HasIndex("CoreUserIdRef")
              .IsUnique(true);


            conf.HasIndex("Email").IsUnique(false);

            var permissions = conf.Metadata.FindNavigation(nameof(User.UserPermissions));
            permissions.SetPropertyAccessMode(PropertyAccessMode.Field);

            var dashboards = conf.Metadata.FindNavigation(nameof(User.Dashboards));
            dashboards.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
