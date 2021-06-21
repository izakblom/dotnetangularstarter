using dotnetstarter.authentication.api.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace dotnetstarter.authentication.api.Application.Commands
{
    [DataContract]
    public class AuthenticateUserCommand : IRequest<DTOToken>
    {
        [DataMember]
        public string Username { get; private set; }
        [DataMember]
        public string Password { get; private set; }

        public AuthenticateUserCommand() { }

        public AuthenticateUserCommand(
            string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
