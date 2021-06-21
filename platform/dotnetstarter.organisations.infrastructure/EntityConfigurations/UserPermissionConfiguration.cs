using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using dotnetstarter.organisations.domain.AggregatesModel.UserAggregate;

namespace dotnetstarter.organisations.infrastructure.EntityConfigurations
{
    class UserPermissionConfiguration : IEntityTypeConfiguration<UserPermission>
    {
        public void Configure(EntityTypeBuilder<UserPermission> conf)
        {
            conf.HasKey("UserId", "PermissionId");

            conf.Property<int>("UserId").IsRequired(true);
            conf.Property<int>("PermissionId").IsRequired(true);

            conf.HasOne(o => o.Permission).WithMany().HasForeignKey("PermissionId");
            conf.HasOne(o => o.User).WithMany(o => o.UserPermissions).HasForeignKey("UserId");

            conf.HasIndex("UserId", "PermissionId").IsUnique(true);
        }
    }
}
