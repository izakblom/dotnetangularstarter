using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using dotnetstarter.organisations.domain.AggregatesModel.RoleAggregate;

namespace dotnetstarter.organisations.infrastructure.EntityConfigurations
{
    class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> conf)
        {
            conf.ToTable("Roles", OrganisationsContext.DEFAULT_SCHEMA);

            conf.HasKey(b => b.Id);

            conf.Ignore(b => b.DomainEvents);

            conf.Property(b => b.Id)
                .ForSqlServerUseSequenceHiLo("buyerseq", OrganisationsContext.DEFAULT_SCHEMA);

            conf.Property<string>("RoleName");
            conf.Property<string>("RoleDescription");

            var permissions = conf.Metadata.FindNavigation(nameof(Role.RolePermissions));
            permissions.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
