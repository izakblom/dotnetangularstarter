using Common.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace dotnetstarter.gateway.domain.AggregatesModel.UserAggregate
{
    /// <summary>
    /// Users authenticated with JWT tokens return the email address and JWTId (unique id issued per user from issuing authority)
    /// Users might sign up more than one time using the same email address but a different provider. ie: username/password and then social. In these events
    /// we want to link 2 logins to the same user (ie: john registered with a social provider like gmail and username/password)
    /// </summary>
    public class User : Entity, IAggregateRoot
    {


        private string _firstName;
        private string _lastName;
        private string _identityNumber;
        private string _mobileNumber;
        private bool _isActive;
        private string _email;
        private bool _verified;
        public string JWTId { get; set; }

        public string FirstName => _firstName;
        public string LastName => _lastName;
        public string IDNumber => _identityNumber;
        public string MobileNumber => _mobileNumber;
        public bool IsActive => _isActive;
        public string Email => _email;

        public bool Verified => _verified;

        public User() : base()
        {

        }

        public User(string firstName, string lastName, string idNumber, string mobileNumber, string email, string jwtId, bool verified) : this()
        {
            _firstName = firstName;
            _lastName = lastName;
            _identityNumber = idNumber;
            _mobileNumber = mobileNumber;
            Validate(email, jwtId);
            _email = email;
            JWTId = jwtId;
            _verified = verified;
        }

        /// <summary>
        /// Run checks to insure data conforms to rules.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="jwtId"></param>
        protected void Validate(string email, string jwtId)
        {
            // check nulls
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException(nameof(email));
            if (string.IsNullOrWhiteSpace(jwtId)) throw new ArgumentNullException(nameof(jwtId));

        }

        /// <summary>
        /// Adds a new alias for the same email address to this user if it does not exist
        /// </summary>
        /// <param name="email"></param>
        /// <param name="jwtId"></param>
        public void AddAlias(string email, string jwtId, bool verified)
        {
            Validate(email, jwtId);

            _email = email;
            _verified = verified;
        }

        public void AddNames(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentNullException(nameof(firstName));
            if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentNullException(nameof(lastName));
            _firstName = firstName;
            _lastName = lastName;
        }

        public void AddIDNumber(string idNumber)
        {
            if (string.IsNullOrWhiteSpace(idNumber)) throw new ArgumentNullException(nameof(idNumber));
            _identityNumber = idNumber;
        }

        public void AddMobileNumber(string mobile)
        {
            if (string.IsNullOrWhiteSpace(mobile)) throw new ArgumentNullException(nameof(mobile));
            _mobileNumber = mobile;
        }

        public bool ProfileComplete()
        {
            return !string.IsNullOrEmpty(_firstName) && !string.IsNullOrEmpty(_lastName) && !string.IsNullOrEmpty(_identityNumber)
                && !string.IsNullOrEmpty(_mobileNumber) && !String.IsNullOrEmpty(_email) && !string.IsNullOrEmpty(JWTId) && _verified;
        }


        public void EnableUser()
        {
            _isActive = true;
        }

        public void DisableUser()
        {
            _isActive = false;
        }
    }
}
