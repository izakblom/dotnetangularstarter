using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace dotnetstarter.organisations.api.Application.Commands.Users
{
    public class GetUserDetailsByIdCommand:IRequest<object>
    {
        public int _userId { get; set; }
        public GetUserDetailsByIdCommand(int userId)
        {
            _userId = userId;
        }
    }
}
