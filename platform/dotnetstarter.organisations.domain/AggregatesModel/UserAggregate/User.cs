using Common.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using dotnetstarter.organisations.domain.AggregatesModel.DashboardAggregate;
using dotnetstarter.organisations.domain.AggregatesModel.RoleAggregate;

namespace dotnetstarter.organisations.domain.AggregatesModel.UserAggregate
{
    /// <summary>
    /// Users authenticated with JWT tokens return the email address and JWTId (unique id issued per user from issuing authority)
    /// Users might sign up more than one time using the same email address but a different provider. ie: username/password and then social. In these events
    /// we want to link 2 logins to the same user (ie: john registered with a social provider like gmail and username/password)
    /// </summary>
    public class User : Entity, IAggregateRoot
    {

        private readonly List<UserPermission> _userPermissions;
        public IReadOnlyCollection<UserPermission> UserPermissions => _userPermissions;

        private readonly List<Dashboard> _dashboards;
        public IReadOnlyCollection<Dashboard> Dashboards => _dashboards;

        private string _firstName;
        private string _lastName;
        private string _identityNumber;
        private string _mobileNumber;
        private bool _isActive;
        private string _email;
        private int _coreUserIdRef;

        public string FirstName => _firstName;
        public string LastName => _lastName;
        public string IDNumber => _identityNumber;
        public string MobileNumber => _mobileNumber;
        public bool IsActive => _isActive;
        public string Email => _email;

        public int CoreUserIdRef => _coreUserIdRef;


        public User() : base()
        {
            _userPermissions = new List<UserPermission>();
            _dashboards = new List<Dashboard>();
        }

        public User(string firstName, string lastName, string idNumber, string mobileNumber, string email, int coreUserIdRef) : this()
        {
            _firstName = firstName;
            _lastName = lastName;
            _identityNumber = idNumber;
            _mobileNumber = mobileNumber;
            Validate(email);
            _email = email;
            _coreUserIdRef = coreUserIdRef;
        }

        /// <summary>
        /// Run checks to insure data conforms to rules.
        /// </summary>
        /// <param name="email"></param>
        protected void Validate(string email)
        {
            // check nulls
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException(nameof(email));

        }

        /// <summary>
        /// Adds a new alias for the same email address to this user if it does not exist
        /// </summary>
        /// <param name="email"></param>
        /// <param name="jwtId"></param>
        public void AddEmail(string email)
        {
            Validate(email);

            _email = email;
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
                && !string.IsNullOrEmpty(_mobileNumber) && !String.IsNullOrEmpty(_email);
        }

        /// <summary>
        /// Acts as a permission toggle. If the permission is assigned, it will be revoked. If it is not assigned, it will be assigned
        /// </summary>
        /// <param name="permission"></param>
        public void AssignRevokePermission(Permission permission)
        {
            int permissionIndex = _userPermissions.FindIndex(upm => { return upm.PermissionId.Equals(permission.Id); });
            if (permissionIndex == -1)
            {
                //Assign
                var userPermission = new UserPermission(permission.Id, Id);
                _userPermissions.Add(userPermission);
            }
            else
            {
                //Revoke
                _userPermissions.RemoveAt(permissionIndex);
            }
        }

        public void AssignPermission(Permission permission)
        {
            int permissionIndex = _userPermissions.FindIndex(upm => { return upm.PermissionId.Equals(permission.Id); });
            if (permissionIndex == -1)
            {
                var userPermission = new UserPermission(permission.Id, Id);
                _userPermissions.Add(userPermission);
            }
        }

        public void RevokePermission(Permission permission)
        {
            int permissionIndex = _userPermissions.FindIndex(upm => { return upm.PermissionId.Equals(permission.Id); });
            if (permissionIndex != -1)
            {
                _userPermissions.RemoveAt(permissionIndex);
            }
        }

        public bool HasPermission(Permission permission)
        {
            return _userPermissions.FindIndex(upm => { return upm.PermissionId.Equals(permission.Id); }) != -1;
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
