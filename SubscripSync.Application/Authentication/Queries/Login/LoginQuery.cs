using MediatR;
using SubscripSync.Application.Authentication.Common;
using SubscripSync.Application.Common.Interfaces.Authentication;
using SubscripSync.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SubscripSync.Application.Authentication.Queries.Login
{
    public record LoginQuery(
        string Email,
        string Password) : IRequest<AuthenticationResult>;

    public class LoginQueryHandler : IRequestHandler<LoginQuery, AuthenticationResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IPasswordHasher _passwordHasher;

        public LoginQueryHandler(
            IUserRepository userRepository,
            IJwtTokenGenerator jwtTokenGenerator,
            IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _passwordHasher = passwordHasher;
        }

        public async Task<AuthenticationResult> Handle(LoginQuery query, CancellationToken cancellationToken)
        {
            // 1. Validate the user exists
            var user = await _userRepository.GetByEmailAsync(query.Email);
            if (user is null)
            {
                throw new Exception("Invalid credentials.");
            }

            // 2. Validate the password
            if (!_passwordHasher.VerifyPassword(query.Password, user.PasswordHash))
            {
                throw new Exception("Invalid credentials."); 
            }

            // 3. Create JWT token
            var token = _jwtTokenGenerator.GenerateToken(user);

            return new AuthenticationResult(user, token);
        }
    }
}
