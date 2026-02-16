using MediatR;
using SubscripSync.Domain.Entities;
using SubscripSync.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SubscripSync.Application.Users.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<Guid>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IUserRepository _repository;

        // In a real app, inject IPasswordHasher
        public CreateUserCommandHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // Basic validation
            var existingUser = await _repository.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                throw new Exception("User with this email already exists.");
            }

            var user = new User
            {
                Email = request.Email,
                // TODO: Hash password
                PasswordHash = request.Password, 
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            await _repository.AddAsync(user);
            return user.Id;
        }
    }
}
