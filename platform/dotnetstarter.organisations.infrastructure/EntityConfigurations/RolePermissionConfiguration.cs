using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using dotnetstarter.organisations.domain.AggregatesModel.RoleAggregate;

namespace dotnetstarter.organisations.infrastructure.EntityConfigurations
{
    class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> conf)
        {
            conf.HasKey("RoleId", "PermissionId");

            conf.Property<int>("RoleId").IsRequired(true);
            conf.Property<int>("PermissionId").IsRequired(true);

            conf.HasOne(o => o.Permission).WithMany().HasForeignKey("PermissionId");
            conf.HasOne(o => o.Role).WithMany(o => o.RolePermissions).HasForeignKey("RoleId");

            conf.HasIndex("RoleId", "PermissionId").IsUnique(true);
        }
    }
}
