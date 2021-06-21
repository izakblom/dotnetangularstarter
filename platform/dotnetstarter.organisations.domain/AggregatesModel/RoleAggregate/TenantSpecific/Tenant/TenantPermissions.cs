using Common.Exceptions;
using Common.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace dotnetstarter.organisations.domain.AggregatesModel.RoleAggregate.TenantSpecific.Tenant
{

    public class TenantPermissions : Enumeration
    {

        //Start tenant-specific permissions at 400 to avoid duplicate permission id's with core permissions
        public static TenantPermissions DEFAULT = new TenantPermissions(400, "Default");

        protected TenantPermissions() { }

        public TenantPermissions(int id, string name)
            : base(id, name)
        {
        }

        public static IEnumerable<TenantPermissions> List()
        {
            return new[] {
                DEFAULT
            };
        }

        public static TenantPermissions FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new GeneralDomainException($"Possible values for {nameof(TenantPermissions)}: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static TenantPermissions From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new GeneralDomainException($"Possible values for {nameof(TenantPermissions)}: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}
