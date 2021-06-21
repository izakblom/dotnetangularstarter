using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace dotnetstarter.authentication.api.Application.Commands
{
    [DataContract]
    public class CreateUserCommand : IRequest<bool>
    {
        [DataMember]
        public string Username { get; private set; }

        public CreateUserCommand() { }

        public CreateUserCommand(
            string username)
        {
            Username = username;
        }
    }
}
