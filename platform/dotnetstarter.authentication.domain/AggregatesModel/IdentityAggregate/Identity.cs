using dotnetstarter.authentication.domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dotnetstarter.authentication.domain.AggregatesModel.IdentityAggregate
{
    public class Identity : EntityBase, IAggregateRoot
    {
        public string Username { get; private set; }
        public string PublicKey { get; private set; }

        private readonly List<IdentityClaim> _claims;
        public IReadOnlyCollection<IdentityClaim> Claims => _claims;

        protected Identity() : base()
        {
            _claims = new List<IdentityClaim>();
        }

        public Identity(string username, string key = "") : this()
        {
            Username = username;
            PublicKey = String.IsNullOrEmpty(key) ? ReferenceGenerator.Generate(20) : key;

        }

        public bool KeyMatch(string publicKey)
        {
            return PublicKey == publicKey;
        }

        public void AddClaim(ClaimType claim)
        {
            if (_claims.Any(c => c.ClaimType == claim))
                return; //don't throw

            _claims.Add(new IdentityClaim(claim));

        }
    }


}
