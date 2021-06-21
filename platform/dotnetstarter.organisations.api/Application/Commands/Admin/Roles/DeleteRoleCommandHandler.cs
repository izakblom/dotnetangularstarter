using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using dotnetstarter.organisations.domain.AggregatesModel.RoleAggregate;

namespace dotnetstarter.organisations.api.Application.Commands.Admin.Roles
{
    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, bool>
    {
        private readonly IRoleRepository roleRepository;

        public DeleteRoleCommandHandler(IRoleRepository roleRepository)
        {
            this.roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
        }

        public async Task<bool> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var role = await roleRepository.FindByIdAsync(request.dtoRole.Id);
                if (role == null)
                    throw new Exception("Role not found");

                roleRepository.Delete(role);

                bool saved = await roleRepository.UnitOfWork.SaveEntitiesAsync();
                return saved;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
