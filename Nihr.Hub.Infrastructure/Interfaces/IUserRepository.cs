using Nihr.Hub.Domain.Entities;

namespace Nihr.Hub.Infrastructure.Interfaces;

public interface IUserRepository
{
    Task<User> GetUser(string email, CancellationToken cancellationToken);
    Task SaveUser(User user, CancellationToken cancellationToken);
}