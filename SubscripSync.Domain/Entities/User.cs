using System;
using SubscripSync.Domain.Common;

namespace SubscripSync.Domain.Entities
{
    public class User : Entity
    {
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
