using SubscripSync.Domain.Entities;

namespace SubscripSync.Application.Authentication.Common
{
    public record AuthenticationResult(
        User User,
        string Token);
}
