using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotnetstarter.organisations.domain.AggregatesModel.RoleAggregate;

namespace dotnetstarter.organisations.infrastructure.EntityConfigurations
{
    class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> conf)
        {
            conf.ToTable("Permissions", OrganisationsContext.LOOKUP_SCHEMA);

            conf.HasKey(o => o.Id);

            conf.Property(o => o.Id)
                .HasDefaultValue(1)
                .ValueGeneratedNever()
                .IsRequired();

            conf.Property(o => o.Name)
                .HasMaxLength(200)
                .IsRequired();

        }
    }

    class PermissionSeed
    {
        public PermissionSeed(ref ModelBuilder builder)
        {
            var list = new List<Permission>();

            Permission.List().ToList().ForEach(i =>
            {
                list.Add(new Permission(i.Id, i.Name));
            });

            builder.Entity<Permission>().HasData(list.ToArray());
        }
    }
}
