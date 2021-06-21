using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using dotnetstarter.organisations.domain.AggregatesModel.DashboardAggregate;

namespace dotnetstarter.organisations.infrastructure.EntityConfigurations
{

    class DashboardConfiguration
        : IEntityTypeConfiguration<Dashboard>
    {
        public void Configure(EntityTypeBuilder<Dashboard> conf)
        {
            conf.ToTable("Dashboards", OrganisationsContext.DEFAULT_SCHEMA);

            conf.HasKey(b => b.Id);

            conf.Ignore(b => b.DomainEvents);

            conf.Property(b => b.Id)
                .ForSqlServerUseSequenceHiLo("buyerseq", OrganisationsContext.DEFAULT_SCHEMA);

            conf.Property<string>("SerializedDashboard").IsRequired();
            conf.Property<string>("Route").IsRequired();

            conf.Property<int>("UserId").IsRequired();
            conf.HasOne(b => b.User)
                .WithMany(u => u.Dashboards)
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.ClientSetNull);



        }
    }
}
