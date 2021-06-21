using dotnetstarter.authentication.api.DTO;
using dotnetstarter.authentication.domain.AggregatesModel.IdentityAggregate;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace dotnetstarter.authentication.api.Application.Commands
{
    public class AuthenticateUserCommandHandler
    : IRequestHandler<AuthenticateUserCommand, DTOToken>
    {
        private readonly IIdentityRepository _identityRepository;
        private readonly IMediator _mediator;
        private IConfiguration _configuration;

        // Using DI to inject infrastructure persistence Repositories
        public AuthenticateUserCommandHandler(IMediator mediator,
                                         IIdentityRepository identityRepository,
                                         IConfiguration configuration)
        {

            _identityRepository = identityRepository ??
                              throw new ArgumentNullException(nameof(identityRepository));

            _mediator = mediator ??
                                     throw new ArgumentNullException(nameof(mediator));

            _configuration = configuration ??
                                     throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<DTOToken> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _identityRepository.FindByUsernameAsync(request.Username);

            if (user == null)
                throw new Exception("Invalid username and/or password.");

            var passwordMatched = user.KeyMatch(request.Password);

            if (!passwordMatched)
                throw new Exception("Invalid username and/or password.");

            //add name and id
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, request.Username),
                new Claim("PublicKey", user.PublicKey),
                new Claim("Username", user.Username)
            };

            //add claims
            user.Claims.ToList().ForEach(c =>
            {
                claims.Add(new Claim(c.ClaimType.Name, "true"));
            });



            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration[$"{Environment.GetEnvironmentVariable("TENANT")}:Authentication:PrivateKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenObj = new DTOToken()
            {
                expiresAt = DateTime.Now.AddHours(2)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration[$"{Environment.GetEnvironmentVariable("TENANT")}:Authentication:Issuer"],
                audience: _configuration[$"{Environment.GetEnvironmentVariable("TENANT")}:Authentication:Audience"],
                claims: claims.ToArray(),
                expires: tokenObj.expiresAt.UtcDateTime,
                signingCredentials: creds);

            tokenObj.token = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenObj;

        }
    }
}
