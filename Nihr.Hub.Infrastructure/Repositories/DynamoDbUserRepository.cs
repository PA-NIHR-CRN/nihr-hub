using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Microsoft.Extensions.Options;
using Nihr.Hub.Domain.Entities;
using Nihr.Hub.Infrastructure.Interfaces;
using Nihr.Hub.Infrastructure.Settings;

namespace Nihr.Hub.Infrastructure.Repositories;

public class DynamoDbUserRepository : IUserRepository
{
    private readonly DynamoDBContext _context;
    private readonly DynamoDBOperationConfig _dynamoDbOperationConfig;

    public DynamoDbUserRepository(IOptions<DynamoDbSettings> options)
    {
        var dynamoTable = options.Value.TableName;

        _dynamoDbOperationConfig = new DynamoDBOperationConfig()
        {
            OverrideTableName = dynamoTable
        };

        var client = new AmazonDynamoDBClient();
        _context = new DynamoDBContext(client);
    }

    public async Task<User?> GetUser(string email, CancellationToken cancellationToken)
    {
        return await _context.LoadAsync<User>(email, _dynamoDbOperationConfig, cancellationToken);
    }

    // Note: SaveAsync handles upserts.
    public async Task SaveUser(User user, CancellationToken cancellationToken)
    {
        user.LastUpdatedDate = DateTimeOffset.Now.ToString("o");
        await _context.SaveAsync(user, _dynamoDbOperationConfig, cancellationToken);
    }
}