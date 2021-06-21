using Common.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace dotnetstarter.organisations.domain.AggregatesModel.RoleAggregate
{
    public class Role : Entity, IAggregateRoot
    {

        private readonly List<RolePermission> _rolePermissions;
        public IReadOnlyCollection<RolePermission> RolePermissions => _rolePermissions;

        private string _roleName;
        private string _roleDescription;

        public string RoleName => _roleName;
        public string RoleDescription => _roleDescription;


        public Role() : base()
        {
            _rolePermissions = new List<RolePermission>();
        }

        public Role(string name, string description) : this()
        {
            _roleName = name;
            _roleDescription = description;
        }

        public void SetName(string roleName)
        {
            _roleName = roleName;
        }

        public void SetDescription(string description)
        {
            _roleDescription = description;
        }

        public void AddPermission(Permission permission)
        {
            int rolePermIndex = _rolePermissions.FindIndex(rpm => { return rpm.PermissionId.Equals(permission.Id); });
            if (rolePermIndex == -1)
                _rolePermissions.Add(new RolePermission(permission.Id, Id));
        }

        public void RemovePermission(Permission permission)
        {
            int rolePermIndex = _rolePermissions.FindIndex(rpm => { return rpm.PermissionId.Equals(permission.Id); });
            if (rolePermIndex != -1)
            {
                _rolePermissions.RemoveAt(rolePermIndex);
            }
        }
    }
}
