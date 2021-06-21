using dotnetstarter.authentication.domain.AggregatesModel.IdentityAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace dotnetstarter.authentication.infrastructure.EntityConfigurations
{
    public class IdentityClaimEntityTypeConfiguration : IEntityTypeConfiguration<IdentityClaim>
    {

        public void Configure(EntityTypeBuilder<IdentityClaim> conf)
        {
            conf.ToTable("identityclaims", IdentityContext.DEFAULT_SCHEMA);

            conf.HasKey(o => o.Id);

            conf.Ignore(b => b.DomainEvents);

            conf.Property<int>("ClaimTypeId").IsRequired();
            conf.Property<Guid>("IdentityId").IsRequired();



            conf.HasIndex("InternalReference")
              .IsUnique(true);

        }

    }


}
