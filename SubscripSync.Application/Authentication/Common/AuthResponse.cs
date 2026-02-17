using System;

namespace SubscripSync.Application.Authentication.Common
{
    public record AuthResponse(
        Guid Id,
        string FirstName,
        string LastName,
        string Email,
        string Token);
}
