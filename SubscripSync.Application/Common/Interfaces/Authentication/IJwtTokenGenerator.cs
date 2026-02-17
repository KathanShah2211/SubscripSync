using SubscripSync.Domain.Entities;
using System.Collections.Generic;
using System.Security.Claims;

namespace SubscripSync.Application.Common.Interfaces.Authentication
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}
