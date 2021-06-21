using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using dotnetstarter.authentication.domain.AggregatesModel.IdentityAggregate;

namespace dotnetstarter.authentication.infrastructure.EntityConfigurations
{
    public class ClaimTypeEntityTypeConfiguration : IEntityTypeConfiguration<ClaimType>
    {
        public void Configure(EntityTypeBuilder<ClaimType> conf)
        {
            conf.ToTable("claimtypes", IdentityContext.LOOKUP_SCHEMA);

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

    class ClaimTypesSeed
    {

        public ClaimTypesSeed(ref ModelBuilder builder)
        {

            var list = new List<ClaimType>();

            ClaimType.List().ToList().ForEach(i =>
            {
                list.Add(new ClaimType(i.Id, i.Name));
            });

            builder.Entity<ClaimType>().HasData(list.ToArray());
        }



    }

}
