using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Nihr.Hub.Domain.Entities;
using Nihr.Hub.Infrastructure.Interfaces;

namespace Nihr.Hub.Infrastructure.Repositories;

public class DynamoDbUserRepository : IUserRepository
{
    private readonly DynamoDBContext _context;

    public DynamoDbUserRepository()
    {
        var client = new AmazonDynamoDBClient();
        _context = new DynamoDBContext(client);
    }

    public async Task<User> GetUser(string email, CancellationToken cancellationToken)
    {
        return await _context.LoadAsync<User>(email, cancellationToken);
    }
}