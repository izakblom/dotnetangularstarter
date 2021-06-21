using dotnetstarter.authentication.domain.AggregatesModel.IdentityAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace dotnetstarter.authentication.infrastructure.EntityConfigurations
{
    public class IdentityEntityTypeConfiguration : IEntityTypeConfiguration<Identity>
    {

        public void Configure(EntityTypeBuilder<Identity> conf)
        {
            conf.ToTable("users", IdentityContext.DEFAULT_SCHEMA);

            conf.HasKey(o => o.Id);

            conf.Ignore(b => b.DomainEvents);

            conf.Property<string>("Username").IsRequired();
            conf.Property<string>("PublicKey").IsRequired();

            var navigation_claims = conf.Metadata.FindNavigation(nameof(Identity.Claims));
            navigation_claims.SetPropertyAccessMode(PropertyAccessMode.Field);


            conf.HasIndex("InternalReference")
              .IsUnique(true);

        }

    }

    class IdentitySeed
    {

        public IdentitySeed(ref ModelBuilder builder)
        {

            var list = new List<Identity>();

            list.Add(new Identity("dotnetstarter_service_user", "examplepw"));

            builder.Entity<Identity>().HasData(list.ToArray());
        }



    }
}
