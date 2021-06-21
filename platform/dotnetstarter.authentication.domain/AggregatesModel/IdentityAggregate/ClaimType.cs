
using dotnetstarter.authentication.domain.Exceptions;
using dotnetstarter.authentication.domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dotnetstarter.authentication.domain.AggregatesModel.IdentityAggregate
{
    public class ClaimType : Enumeration
    {
        public static ClaimType SystemAdministrator = new ClaimType(1, nameof(SystemAdministrator).ToLowerInvariant());
        public static ClaimType Integrator = new ClaimType(2, nameof(Integrator).ToLowerInvariant());
        public static ClaimType ServiceProvider = new ClaimType(3, nameof(ServiceProvider).ToLowerInvariant());

        protected ClaimType() { }

        public ClaimType(int id, string name)
            : base(id, name)
        {
        }

        public static IEnumerable<ClaimType> List()
        {
            return new[] { SystemAdministrator, Integrator, ServiceProvider };
        }

        public static ClaimType FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new AuthenticationDomainException($"Possible values for ClaimType: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static ClaimType From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new AuthenticationDomainException($"Possible values for ClaimType: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

    }
}
