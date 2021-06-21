using Common.Utilities.CustomAttributes;
using dotnetstarter.gateway.api.Application.Services;
using dotnetstarter.gateway.domain.AggregatesModel.UserAggregate;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using DTOUserProfile = dotnetstarter.organisations.common.DataObjects.DTOUserProfile;

namespace dotnetstarter.gateway.api.Application.Commands.Users
{
    public class GetUserProfileFormCommandHandler : IRequestHandler<GetUserProfileFormCommand, DTODynamicFormsStructure>
    {
        private IUserRepository _userRepository;
        private IInternalAPIService _internalAPIService;

        public GetUserProfileFormCommandHandler(IUserRepository userRepository, IInternalAPIService internalAPIService)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _internalAPIService = internalAPIService ?? throw new ArgumentNullException(nameof(internalAPIService));
        }

        public async Task<DTODynamicFormsStructure> Handle(GetUserProfileFormCommand request, CancellationToken cancellationToken)
        {
            try
            {
                User user = await _userRepository.FindByProviderUID(request.ClaimsUserJWTId);
                var form = new DTODynamicFormsStructure();
                form = DynamicFormAttributeHelper.BuildFromType(typeof(DTOUserProfile));
                if (user == null)
                { //User does not exist, return form elements empty
                    form.elements.Find(el => { return el.key.Equals(nameof(DTOUserProfile.Email)); }).value = request.ClaimsUserEmail;


                }
                else
                {
                    form.elements.Find(el => { return el.key.Equals(nameof(DTOUserProfile.FirstName)); }).value = user.FirstName;
                    form.elements.Find(el => { return el.key.Equals(nameof(DTOUserProfile.LastName)); }).value = user.LastName;
                    form.elements.Find(el => { return el.key.Equals(nameof(DTOUserProfile.MobileNumber)); }).value = user.MobileNumber;
                    form.elements.Find(el => { return el.key.Equals(nameof(DTOUserProfile.IDNumber)); }).value = user.IDNumber;
                    form.elements.Find(el => { return el.key.Equals(nameof(DTOUserProfile.Email)); }).value = request.ClaimsUserEmail;
                }
                return form;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
