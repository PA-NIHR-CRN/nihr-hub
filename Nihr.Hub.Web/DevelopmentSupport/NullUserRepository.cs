using Microsoft.Extensions.Options;
using Nihr.Hub.Domain.Entities;
using Nihr.Hub.Infrastructure.Interfaces;

internal class NullUserRepository(IOptionsSnapshot<DevelopmentModeUserRepositorySettings> options) : IUserRepository
{
    public Task<User?> GetUser(string email, CancellationToken cancellationToken)
    {
        return Task.FromResult<User?>(new User
        {
            Email = email,
            AupAcceptedVersion = options.Value.AupAcceptedVersion ?? string.Empty,
            AupAcceptedDate = options.Value.AupAcceptedDate?.ToString("o") ?? string.Empty
        }
        );
    }

    public Task SaveUser(User user, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}