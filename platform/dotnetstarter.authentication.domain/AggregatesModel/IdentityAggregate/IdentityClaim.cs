using dotnetstarter.authentication.domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace dotnetstarter.authentication.domain.AggregatesModel.IdentityAggregate
{
    public class IdentityClaim : EntityBase
    {
        public ClaimType ClaimType { get; private set; }
        private int _claimTypeId;

        public Identity Identity { get; private set; }
        private Guid _identityId;

        protected IdentityClaim() : base()
        {

        }

        public IdentityClaim(ClaimType claim) : this()
        {
            _claimTypeId = claim.Id;
        }
    }


}
